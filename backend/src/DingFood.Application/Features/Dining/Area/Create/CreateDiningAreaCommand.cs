using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Area.Create
{
    public sealed record CreateDiningAreaCommand(
        long BranchId,
        string Name) : ICommand<long>;
}
