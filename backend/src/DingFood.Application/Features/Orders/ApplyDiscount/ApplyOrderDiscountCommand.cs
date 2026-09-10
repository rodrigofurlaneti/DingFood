using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.ApplyDiscount;

public sealed record ApplyOrderDiscountCommand(long CustomerOrderId, decimal DiscountAmount) : ICommand;
