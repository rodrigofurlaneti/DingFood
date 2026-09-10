using FluentValidation;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Auth.CustomerLogin;

namespace DingFood.Application.Features.Auth.CustomerRefresh;

public sealed record CustomerRefreshTokenCommand(string RefreshToken) : ICommand<CustomerLoginResponse>;

public sealed class CustomerRefreshTokenCommandValidator : AbstractValidator<CustomerRefreshTokenCommand>
{
    public CustomerRefreshTokenCommandValidator() => RuleFor(x => x.RefreshToken).NotEmpty().MaximumLength(500);
}
