using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Branches.GetByCompany;

public sealed record GetBranchesByCompanyQuery(long CompanyId) : IQuery<IReadOnlyCollection<BranchResponse>>;
