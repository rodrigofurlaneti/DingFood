using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Shipping;

public sealed record IfoodShippingCancellationReasonResponse(string CancelCodeId, string Description);

public sealed record GetIfoodShippingCancellationReasonsQuery(long Id) : IQuery<IReadOnlyCollection<IfoodShippingCancellationReasonResponse>>;
