using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Shipping;

public sealed record CancelIfoodShippingDeliveryCommand(long Id, string Reason, int CancellationCode) : ICommand;
