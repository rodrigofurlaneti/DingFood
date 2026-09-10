using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Billing.RefundSale;

public sealed record RefundSaleCommand(long SaleId, long EmployeeId, string? Reason) : ICommand;
