using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Area.Update
{
    public sealed record UpdateDiningAreaCommand(
        long Id,
        string Name) : ICommand;
}
