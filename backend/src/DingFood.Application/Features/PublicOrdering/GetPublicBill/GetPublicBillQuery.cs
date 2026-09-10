using MediatR;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
namespace DingFood.Application.Features.PublicOrdering.GetPublicBill
{
    public sealed record GetPublicBillQuery(Guid Token) : IQuery<PublicBillResponse>;
}
