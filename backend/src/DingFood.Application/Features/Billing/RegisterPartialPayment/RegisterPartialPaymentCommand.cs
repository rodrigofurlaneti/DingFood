using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Billing.RegisterPartialPayment;

public sealed record RegisterPartialPaymentCommand(
    long CustomerOrderId,
    long CashSessionId,
    long EmployeeId,
    long PaymentMethodId,
    decimal Amount,
    string? AuthorizationCode,
    string? PayerName) : ICommand<long>;
