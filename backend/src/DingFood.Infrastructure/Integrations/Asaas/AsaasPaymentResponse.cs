using System.Text.Json.Serialization;
namespace DingFood.Infrastructure.Integrations.Asaas
{
    public sealed record AsaasPaymentResponse(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("customer")] string Customer,
        [property: JsonPropertyName("value")] decimal Value,
        [property: JsonPropertyName("netValue")] decimal? NetValue,
        [property: JsonPropertyName("billingType")] string BillingType,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("dueDate")] string DueDate,
        [property: JsonPropertyName("invoiceUrl")] string? InvoiceUrl,
        [property: JsonPropertyName("bankSlipUrl")] string? BankSlipUrl);
}
