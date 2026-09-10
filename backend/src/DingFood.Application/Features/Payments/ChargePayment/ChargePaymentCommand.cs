using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Payments;

namespace DingFood.Application.Features.Payments.ChargePayment;

public sealed record ChargePaymentCommand(
    long SaleId,
    decimal Amount,
    PaymentGatewayMethod Method,
    string? CustomerDocument) : ICommand<ChargePaymentResponse>;

public sealed record ChargePaymentResponse(
    string GatewayTransactionId,
    string Status,
    string? QrCodePayload);
