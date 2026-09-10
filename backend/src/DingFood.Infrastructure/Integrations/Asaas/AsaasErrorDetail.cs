using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public record AsaasErrorDetail(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("description")] string Description
    );
}
