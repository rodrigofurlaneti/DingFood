using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Comandas.Settings;

public sealed record SetComandaDefaultLimitCommand(long BranchId, decimal DefaultLimitAmount) : ICommand;
