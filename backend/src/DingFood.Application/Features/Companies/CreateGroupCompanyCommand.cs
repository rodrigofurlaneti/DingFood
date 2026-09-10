using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Security;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Companies;

public sealed record CreateGroupCompanyCommand(string LegalName, string TradeName, string Cnpj, string BranchName) : ICommand<long>;

internal sealed class CreateGroupCompanyCommandHandler(ICompanyAccessService access, ICurrentUserService user,
    ICurrentTenantService tenant, IBusinessGroupRepository groups, ICompanyRepository companies)
    : ICommandHandler<CreateGroupCompanyCommand, long>
{
    public async Task<Result<long>> Handle(CreateGroupCompanyCommand request, CancellationToken ct)
    {
        var current = await access.ResolveAsync(user.UserId, tenant.CompanyId ?? 0, ct);
        if (current is null || !current.Roles.Contains("Administrador"))
            return Result.Failure<long>(new Error("Company.Forbidden", "Somente administradores podem criar empresas no grupo."));
        var group = await groups.GetByIdAsync(current.BusinessGroupId, ct);
        if (group is null) return Result.Failure<long>(new Error("BusinessGroup.NotFound", "Grupo não encontrado."));
        var cnpj = new string((request.Cnpj ?? "").Where(char.IsDigit).ToArray());
        if (cnpj.Length != 14 || string.IsNullOrWhiteSpace(request.LegalName) || string.IsNullOrWhiteSpace(request.TradeName) || request.LegalName.Length > 200 || request.TradeName.Length > 150 ||
            string.IsNullOrWhiteSpace(request.BranchName) || request.BranchName.Length > 150)
            return Result.Failure<long>(new Error("Company.InvalidInput", "Informe CNPJ, razão social, nome e filial válidos."));
        if (await companies.ExistsByCnpjAsync(cnpj, ct))
            return Result.Failure<long>(new Error("Company.AlreadyExists", "CNPJ já cadastrado."));
        var result = Company.Create(request.LegalName, request.TradeName, cnpj, null, null);
        if (result.IsFailure) return Result.Failure<long>(result.Error);
        var association = result.Value.AssignToGroup(group);
        if (association.IsFailure) return Result.Failure<long>(association.Error);
        return Result.Success(await groups.AddCompanyWithAdministratorAsync(result.Value, user.UserId, request.BranchName, ct));
    }
}
