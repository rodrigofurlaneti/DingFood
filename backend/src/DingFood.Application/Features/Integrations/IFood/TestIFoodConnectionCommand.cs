using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood;

public sealed record TestIfoodConnectionCommand(long CompanyId) : ICommand<TestIfoodConnectionResponse>;

public sealed record TestIfoodConnectionResponse(bool Success, string? ErrorMessage);
