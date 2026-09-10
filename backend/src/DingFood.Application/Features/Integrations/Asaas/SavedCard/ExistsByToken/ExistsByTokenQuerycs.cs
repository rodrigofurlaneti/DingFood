using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.ExistsByToken
{
    public sealed record ExistsByTokenQuery(
        string CreditCardToken) : IQuery<bool>;
}
