using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood;

internal sealed class GetIfoodMerchantMappingsQueryHandler(
    IBranchRepository branchRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetIfoodMerchantMappingsQuery, IReadOnlyCollection<IfoodMerchantMappingResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<IfoodMerchantMappingResponse>>> Handle(
        GetIfoodMerchantMappingsQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetIfoodMerchantMappingsQueryHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                var branches = await branchRepository.GetByCompanyAsync(request.CompanyId, cancellationToken);
                var mappings = await mappingRepository.GetByCompanyAsync(request.CompanyId, cancellationToken);

                // Uma linha por filial ativa — inclusive as que ainda não têm MerchantId
                // configurado, pra tela deixar isso visível em vez de esconder.
                IReadOnlyCollection<IfoodMerchantMappingResponse> response = branches
                    .Where(b => b.IsActive)
                    .Select(b => mappings.TryGetValue(b.Id, out var mapping)
                        ? new IfoodMerchantMappingResponse(b.Id, b.Name, mapping.MerchantId, mapping.MerchantUuid)
                        : new IfoodMerchantMappingResponse(b.Id, b.Name, null, null))
                    .ToList();

                return Result.Success(response);
            });
    }
}
