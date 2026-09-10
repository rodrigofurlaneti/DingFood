using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Constants;

namespace DingFood.Application.Features.Orders.Open;

public sealed record OpenOrderCommand(
    long BranchId,
    long? DiningTableId,
    long? ComandaId,
    long EmployeeId,
    int? GuestCount,
    string? Notes,
    long OrderTypeId = OrderTypeIds.Mesa,
    string? CustomerName = null,
    string? CustomerPhone = null,
    string? DeliveryAddress = null) : ICommand<long>;
