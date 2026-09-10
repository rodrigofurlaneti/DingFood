using MediatR;
using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.TransferTableItems
{
    public sealed record TransferTableItemsCommand(
        long SourceCustomerOrderId,
        long TargetCustomerOrderId,
        IReadOnlyCollection<long> CustomerOrderItemIds,
        long SourceDiningTableId,
        long TargetDiningTableId,
        long ActorEmployeeId) : ICommand<Unit>;
}