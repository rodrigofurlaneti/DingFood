using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public sealed record AsaasErrorItem(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("description")] string Description);
}
