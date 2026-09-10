using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public record AsaasCreditCardData(
            [property: JsonPropertyName("creditCardNumber")] string CreditCardNumber,
            [property: JsonPropertyName("creditCardBrand")] string CreditCardBrand,
            [property: JsonPropertyName("creditCardToken")] string? CreditCardToken // Token para futuras compras
        );
}
