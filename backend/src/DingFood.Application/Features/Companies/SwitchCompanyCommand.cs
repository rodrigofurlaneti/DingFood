using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Security;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Companies;

public sealed record SwitchCompanyCommand(long TargetCompanyId) : ICommand<SwitchCompanyResponse>;
public sealed record SwitchCompanyResponse(string AccessToken, DateTime AccessTokenExpiresAt, long CompanyId, long BusinessGroupId, long? EmployeeId);
internal sealed class SwitchCompanyCommandHandler(ICompanyAccessService access, ICurrentUserService user,
    IBusinessGroupRepository groups, IJwtTokenProvider tokens) : ICommandHandler<SwitchCompanyCommand, SwitchCompanyResponse>
{
    public async Task<Result<SwitchCompanyResponse>> Handle(SwitchCompanyCommand request, CancellationToken ct)
    {
        var company = await access.ResolveAsync(user.UserId, request.TargetCompanyId, ct);
        var identity = await groups.GetUserHomeAsync(user.UserId, ct);
        if (company is null || identity is null)
            return Result.Failure<SwitchCompanyResponse>(new Error("Company.Forbidden", "Empresa não autorizada."));
        var token = tokens.GenerateCompanyToken(identity.Value.User, company);
        return Result.Success(new SwitchCompanyResponse(token.Token, token.ExpiresAt, company.CompanyId, company.BusinessGroupId, company.EmployeeId));
    }
}
