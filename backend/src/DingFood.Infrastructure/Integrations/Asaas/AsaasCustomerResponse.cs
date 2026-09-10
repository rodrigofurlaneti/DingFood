using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public sealed record AsaasCustomerResponse(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("cpfCnpj")] string CpfCnpj,
        [property: JsonPropertyName("email")] string Email);
}
