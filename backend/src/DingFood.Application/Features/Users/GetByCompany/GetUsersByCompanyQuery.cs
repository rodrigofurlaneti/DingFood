using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Users.GetByCompany;

public sealed record GetUsersByCompanyQuery(long CompanyId) : IQuery<IReadOnlyCollection<UserResponse>>;
