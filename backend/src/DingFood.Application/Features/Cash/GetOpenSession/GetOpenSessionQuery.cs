using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cash.GetOpenSession;

public sealed record GetOpenSessionQuery(long CashRegisterId) : IQuery<CashSessionResponse>;
