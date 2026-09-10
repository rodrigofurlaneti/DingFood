using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAppUser.GetById
{
    public sealed record GetCustomerAppUserByIdQuery(long Id) : IQuery<CustomerAppUserResponse>;
}
