using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Polling
{
    public sealed record PollKeetaEventsCommand(
        long CompanyId,
        long BranchId,
        IReadOnlyList<string>? MerchantIds = null) : ICommand<PollKeetaEventsResponse>;
}
