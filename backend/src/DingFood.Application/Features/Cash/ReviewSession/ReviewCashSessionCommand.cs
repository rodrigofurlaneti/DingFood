using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cash.ReviewSession;

public sealed record ReviewCashSessionCommand(long CashSessionId) : ICommand;
