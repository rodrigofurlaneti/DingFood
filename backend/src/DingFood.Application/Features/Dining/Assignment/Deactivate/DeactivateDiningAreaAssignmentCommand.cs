using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Assignment.Deactivate
{
    public sealed record DeactivateDiningAreaAssignmentCommand(long Id) : ICommand;
}
