using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Update
{
    public sealed record UpdateKeetaIntegrationOrderCommand(
        long Id,
        long CompanyId,
        string? Status = null) : ICommand;
}
