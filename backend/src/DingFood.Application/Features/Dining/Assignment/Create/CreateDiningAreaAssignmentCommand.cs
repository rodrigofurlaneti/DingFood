using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Assignment.Create
{
    public sealed record CreateDiningAreaAssignmentCommand(
        long DiningAreaId,
        long EmployeeId,
        DateTime StartAt) : ICommand<long>;
}
