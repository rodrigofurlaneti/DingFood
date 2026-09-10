using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Shipping;

public sealed record ConfirmUserAddressCommand(long IfoodOrderId) : ICommand;
