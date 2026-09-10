using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Delete
{
    public sealed record DeleteKeetaIntegrationOrderCommand(
        long Id,
        long CompanyId) : ICommand;
}
