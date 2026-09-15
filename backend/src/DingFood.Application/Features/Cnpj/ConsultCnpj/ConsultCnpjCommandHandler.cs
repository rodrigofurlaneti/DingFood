using DingFood.Application.Abstractions.Integrations.Cnpja;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Constants;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Cnpj.ConsultCnpj;

internal sealed class ConsultCnpjCommandHandler(
    ICnpjaClient cnpjaClient,
    ICnpjQueryRepository cnpjQueryRepository,
    ICnpjaOptions cnpjaOptions,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<ConsultCnpjCommand, CnpjResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<CnpjResponse>> Handle(
        ConsultCnpjCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(ConsultCnpjCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                // 1. Normaliza e valida antes de gastar cota da API (5 consultas/min por IP).
                var taxId = CnpjValidator.Normalize(request.TaxId);

                if (!CnpjValidator.IsValid(taxId))
                {
                    return Result.Failure<CnpjResponse>(
                        new Error("Cnpj.Invalid", "O CNPJ informado é inválido."));
                }

                // 2. Busca com tracking: se houver hit, pode precisar de Refresh no mesmo objeto.
                var existing = await cnpjQueryRepository.GetByTaxIdForUpdateAsync(taxId, cancellationToken);

                // 3. Cache quente: devolve sem tocar na rede.
                if (existing is not null
                    && !request.ForceRefresh
                    && !existing.IsStale(cnpjaOptions.CacheTtlDays))
                {
                    return Result.Success(CnpjMapper.ToResponse(existing, null, fromCache: true, staleData: false));
                }

                // 4. Cache frio (ou refresh forçado): consulta a CNPJá.
                var remote = await cnpjaClient.GetOfficeAsync(taxId, cancellationToken);

                if (remote.IsFailure)
                {
                    // 4a. Fornecedor fora do ar / rate limit, mas temos um dado antigo:
                    //     serve o antigo marcado como stale em vez de quebrar a tela.
                    if (existing is not null)
                    {
                        return Result.Success(
                            CnpjMapper.ToResponse(existing, null, fromCache: true, staleData: true));
                    }

                    // 4b. Sem nada em cache: propaga o erro do client.
                    return Result.Failure<CnpjResponse>(remote.Error);
                }

                var snapshot = CnpjMapper.ToSnapshot(remote.Value.Payload, remote.Value.RawJson);

                // 5. Grava (insert ou update in-place, o TaxId é único).
                if (existing is null)
                {
                    var created = CnpjQuery.Create(taxId, snapshot);

                    if (created.IsFailure)
                        return Result.Failure<CnpjResponse>(created.Error);

                    await cnpjQueryRepository.AddAsync(created.Value, cancellationToken);
                    existing = created.Value;
                }
                else
                {
                    existing.Refresh(snapshot);
                }

                await unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(
                    CnpjMapper.ToResponse(existing, remote.Value.Payload, fromCache: false, staleData: false));
            });
    }
}
