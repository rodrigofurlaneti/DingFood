using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public sealed record CreditCardHolderInfoRequest(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("cpfCnpj")] string CpfCnpj,
        [property: JsonPropertyName("postalCode")] string PostalCode,
        [property: JsonPropertyName("addressNumber")] string AddressNumber,
        [property: JsonPropertyName("phone")] string Phone);
}
