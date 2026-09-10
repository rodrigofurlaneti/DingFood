using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Table.Deactivate
{
    public sealed record DeactivateDiningAreaTableCommand(long Id) : ICommand;
}
