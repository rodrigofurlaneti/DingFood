using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<LoginResponse>;
