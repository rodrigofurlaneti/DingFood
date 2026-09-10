using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.OrderOrigin.Create
{
    public sealed record CreateOrderOriginCommand(
        long? CompanyId,
        long? BranchId,
        string Name) : ICommand<long>;
}
