using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken, long? CompanyId = null) : ICommand<LoginResponse>;
