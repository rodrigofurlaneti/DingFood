using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Messages.Create
{
    public sealed record CreateWaiterMessageCommand(
        long BranchId,
        long SenderEmployeeId,
        long? RecipientEmployeeId,
        long DiningAreaId, 
        string Message
    ) : ICommand<long>;
}