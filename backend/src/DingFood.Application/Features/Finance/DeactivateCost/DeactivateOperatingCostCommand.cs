using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Finance.DeactivateCost;

public sealed record DeactivateOperatingCostCommand(long OperatingCostId) : ICommand;
