using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Catalog.GetProductById
{
    public sealed record GetProductByIdQuery(long ProductId) : IQuery<ProductResponse>;
}
