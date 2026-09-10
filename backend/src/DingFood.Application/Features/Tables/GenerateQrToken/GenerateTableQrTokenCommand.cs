using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Tables.GenerateQrToken;

public sealed record GenerateTableQrTokenCommand(long DiningTableId) : ICommand<Guid>;
