using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAppUser.GetByBranchId
{
    public sealed record GetCustomerAppUsersByBranchIdQuery(long BranchId) : IQuery<IEnumerable<CustomerAppUserResponse>>;
}
