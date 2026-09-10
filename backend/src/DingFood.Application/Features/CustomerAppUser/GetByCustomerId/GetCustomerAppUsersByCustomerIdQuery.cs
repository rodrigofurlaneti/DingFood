using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.CustomerAppUser.GetById;
namespace DingFood.Application.Features.CustomerAppUser.GetByCustomerId
{
    public sealed record GetCustomerAppUsersByCustomerIdQuery(long CustomerId) : IQuery<IEnumerable<CustomerAppUserResponse>>;
}
