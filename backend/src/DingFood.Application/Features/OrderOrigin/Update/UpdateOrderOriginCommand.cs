using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.OrderOrigin.Update
{
    public sealed record UpdateOrderOriginCommand(
        long Id,
        long? CompanyId,
        long? BranchId,
        string Name,
        bool IsActive) : ICommand;
}
