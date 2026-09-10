using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.Delete
{
    public sealed record DeleteKeetaIntegrationOrderEventLogCommand(
        long Id,
        long CompanyId) : ICommand;
}
