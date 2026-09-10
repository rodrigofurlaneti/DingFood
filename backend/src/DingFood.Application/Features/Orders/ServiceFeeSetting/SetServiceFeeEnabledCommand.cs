using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.ServiceFeeSetting;

public sealed record SetServiceFeeEnabledCommand(long BranchId, bool Enabled) : ICommand;
