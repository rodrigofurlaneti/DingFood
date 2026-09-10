using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Auth.CustomerLogin;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Auth.CustomerRefresh;

internal sealed class CustomerRefreshTokenCommandHandler(
    ICustomerRefreshTokenRepository tokens,
    ICustomerAppUserRepository customers,
    IJwtTokenProvider jwt,
    ILogTrackerRepository logs,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<CustomerRefreshTokenCommand, CustomerLoginResponse>(logs, unitOfWork)
{
    public override Task<Result<CustomerLoginResponse>> Handle(CustomerRefreshTokenCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(nameof(CustomerRefreshTokenCommandHandler), nameof(Handle), null, async _ =>
        {
            var stored = await tokens.GetByTokenForUpdateAsync(request.RefreshToken, cancellationToken);
            var customer = stored is not null && stored.IsValid()
                ? await customers.GetByIdAsync(stored.CustomerAppUserId, cancellationToken) : null;
            if (customer is null || !customer.IsActive)
                return Result.Failure<CustomerLoginResponse>(new Error("Auth.InvalidRefreshToken", "Sessão expirada. Entre novamente."));

            var tokenValue = jwt.GenerateRefreshToken();
            var expiresAt = DateTime.Now.AddDays(7);
            var token = CustomerRefreshToken.Create(customer.Id, tokenValue, expiresAt);
            if (token.IsFailure) return Result.Failure<CustomerLoginResponse>(token.Error);
            var access = jwt.GenerateCustomerToken(customer, ["Customer"], []);
            stored!.Revoke();
            await tokens.AddAsync(token.Value, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(new CustomerLoginResponse(access.Token, access.ExpiresAt, tokenValue,
                expiresAt, customer.UserName, customer.CustomerId ?? 0, customer.CompanyId));
        });
}
