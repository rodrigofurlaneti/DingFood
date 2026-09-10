using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Financial;

public sealed record GetIfoodReconciliationOnDemandStatusQuery(long BranchId, string RequestId)
    : IQuery<IfoodReconciliationOnDemandStatusResponse>;
