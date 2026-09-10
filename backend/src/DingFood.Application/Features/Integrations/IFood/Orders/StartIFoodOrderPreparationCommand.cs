using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

public sealed record StartIfoodOrderPreparationCommand(long IfoodOrderId) : ICommand;
