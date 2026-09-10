using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Auth.Refresh;

internal sealed class RefreshTokenCommandHandler : BaseCommandHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAppUserRepository _userRepository;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DingFood.Application.Abstractions.Tenancy.ICompanyAccessService? _companyAccess;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IAppUserRepository userRepository,
        IJwtTokenProvider jwtTokenProvider,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork,
        DingFood.Application.Abstractions.Tenancy.ICompanyAccessService? companyAccess = null)
        : base(logRepository, unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenProvider = jwtTokenProvider;
        _unitOfWork = unitOfWork;
        _companyAccess = companyAccess;
    }

    public override Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(nameof(RefreshTokenCommandHandler), nameof(Handle), null, async (userIdBox) =>
        {
            var stored = await _refreshTokenRepository.GetByTokenForUpdateAsync(request.RefreshToken, cancellationToken);
            if (stored is null || !stored.IsValid())
                return Result.Failure<LoginResponse>(
                    new Error("Auth.InvalidRefreshToken", "Refresh token is invalid, expired or revoked."));

            var user = await _userRepository.GetByIdAsync(stored.AppUserId, cancellationToken);
            if (user is null || !user.IsActive)
                return Result.Failure<LoginResponse>(
                    new Error("Auth.InvalidRefreshToken", "Refresh token is invalid, expired or revoked."));

            userIdBox.Value = user.Id;

            var company = _companyAccess is null ? null : await _companyAccess.ResolveAsync(user.Id, request.CompanyId ?? user.CompanyId, cancellationToken);
            if (_companyAccess is not null && company is null)
                return Result.Failure<LoginResponse>(new Error("Company.Forbidden", "Empresa não autorizada."));
            stored.Revoke();

            var roles = await _userRepository.GetRoleNamesAsync(user.Id, cancellationToken);
            var permissions = await _userRepository.GetPermissionCodesAsync(user.Id, cancellationToken);
            var accessToken = company is null ? _jwtTokenProvider.GenerateToken(user, roles, permissions)
                : _jwtTokenProvider.GenerateCompanyToken(user, company);

            var newTokenValue = _jwtTokenProvider.GenerateRefreshToken();
            var newTokenExpiresAt = DateTime.Now.AddDays(7);
            var newToken = RefreshToken.Create(user.Id, newTokenValue, newTokenExpiresAt);
            if (newToken.IsFailure)
                return Result.Failure<LoginResponse>(newToken.Error);

            await _refreshTokenRepository.AddAsync(newToken.Value, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(new LoginResponse(
                accessToken.Token, accessToken.ExpiresAt,
                newTokenValue, newTokenExpiresAt,
                user.UserName, company?.CompanyId ?? user.CompanyId, company is null ? user.EmployeeId : company.EmployeeId,
                company?.BusinessGroupId, user.CompanyId));
        });
}
