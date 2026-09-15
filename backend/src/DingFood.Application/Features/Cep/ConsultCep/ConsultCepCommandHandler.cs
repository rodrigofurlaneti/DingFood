using DingFood.Application.Abstractions.Integrations.ViaCep;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Constants;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Cep.ConsultCep;

internal sealed class ConsultCepCommandHandler(
    IViaCepClient viaCepClient,
    ICepQueryRepository cepQueryRepository,
    IViaCepOptions viaCepOptions,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<ConsultCepCommand, CepResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<CepResponse>> Handle(
        ConsultCepCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(ConsultCepCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                // 1. Normaliza e valida antes de chamar o serviço — o ViaCEP bloqueia acesso
                //    por uso massivo, e CEP malformado devolve 400 de qualquer forma.
                var cep = CepValidator.Normalize(request.Cep);

                if (!CepValidator.IsValid(cep))
                {
                    return Result.Failure<CepResponse>(
                        new Error("Cep.Invalid", "O CEP informado é inválido."));
                }

                // 2. Busca com tracking: se houver hit, pode precisar de Refresh no mesmo objeto.
                var existing = await cepQueryRepository.GetByCepForUpdateAsync(cep, cancellationToken);

                // 3. Cache quente: devolve sem tocar na rede.
                if (existing is not null
                    && !request.ForceRefresh
                    && !existing.IsStale(viaCepOptions.CacheTtlDays))
                {
                    return Result.Success(CepMapper.ToResponse(existing, fromCache: true, staleData: false));
                }

                // 4. Cache frio (ou refresh forçado): consulta o ViaCEP.
                var remote = await viaCepClient.GetAddressAsync(cep, cancellationToken);

                if (remote.IsFailure)
                {
                    // 4a. Serviço fora do ar, mas temos um dado antigo: endereço de CEP muda
                    //     muito pouco, então servir o antigo marcado como stale é melhor
                    //     do que travar o formulário do usuário.
                    if (existing is not null)
                    {
                        return Result.Success(
                            CepMapper.ToResponse(existing, fromCache: true, staleData: true));
                    }

                    // 4b. Sem nada em cache: propaga o erro do client.
                    return Result.Failure<CepResponse>(remote.Error);
                }

                var snapshot = CepMapper.ToSnapshot(remote.Value.Payload, remote.Value.RawJson);

                // 5. Grava (insert ou update in-place, o CEP é único).
                if (existing is null)
                {
                    var created = CepQuery.Create(cep, snapshot);

                    if (created.IsFailure)
                        return Result.Failure<CepResponse>(created.Error);

                    await cepQueryRepository.AddAsync(created.Value, cancellationToken);
                    existing = created.Value;
                }
                else
                {
                    existing.Refresh(snapshot);
                }

                await unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(CepMapper.ToResponse(existing, fromCache: false, staleData: false));
            });
    }
}
