using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.Update
{
    public sealed record UpdateKeetaIntegrationOrderEventLogCommand(
        long Id,
        long CompanyId,
        bool MarkAsProcessed = false,
        string? ErrorMessage = null) : ICommand;
}
