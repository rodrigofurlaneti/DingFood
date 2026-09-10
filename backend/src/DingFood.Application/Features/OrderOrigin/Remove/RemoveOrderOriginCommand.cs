using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.OrderOrigin.Remove
{
    public sealed record RemoveOrderOriginCommand(long Id) : ICommand;
}
