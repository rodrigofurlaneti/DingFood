using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public sealed record CreditCardRequest(
        [property: JsonPropertyName("holderName")] string HolderName,
        [property: JsonPropertyName("number")] string Number,
        [property: JsonPropertyName("expiryMonth")] string ExpiryMonth,
        [property: JsonPropertyName("expiryYear")] string ExpiryYear,
        [property: JsonPropertyName("ccv")] string Ccv);
}
