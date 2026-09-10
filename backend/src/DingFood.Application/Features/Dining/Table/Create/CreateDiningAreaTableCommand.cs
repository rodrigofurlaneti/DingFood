using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Table.Create
{
    public sealed record CreateDiningAreaTableCommand(
        long DiningAreaId,
        long DiningTableId) : ICommand<long>;
}
