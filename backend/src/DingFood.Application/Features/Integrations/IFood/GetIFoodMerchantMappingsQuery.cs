using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood;

public sealed record GetIfoodMerchantMappingsQuery(long CompanyId) : IQuery<IReadOnlyCollection<IfoodMerchantMappingResponse>>;
