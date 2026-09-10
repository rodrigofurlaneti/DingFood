using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAppUser.GetByCompanyId
{
    public sealed record GetCustomerAppUsersByCompanyIdQuery(long CompanyId) : IQuery<IEnumerable<CustomerAppUserResponse>>;
}
