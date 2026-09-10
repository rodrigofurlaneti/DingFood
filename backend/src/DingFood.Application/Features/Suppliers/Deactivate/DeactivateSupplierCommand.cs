using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Suppliers.Deactivate;

public sealed record DeactivateSupplierCommand(long SupplierId) : ICommand;
