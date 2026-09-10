using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Table.Update
{
    public sealed record UpdateDiningAreaTableCommand(
        long Id,
        long DiningAreaId,
        long DiningTableId) : ICommand;
}
