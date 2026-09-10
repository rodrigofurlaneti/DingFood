using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class ProductStockRepository(AppDbContext context) : IProductStockRepository
{
    public async Task<ProductStock?> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default)
        => await context.Set<ProductStock>()
            .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);

    public void AddMovement(StockMovement movement)
    {
        context.Set<StockMovement>().Add(movement);
    }

    public void Add(ProductStock stockSnapshot)
    {
        context.Set<ProductStock>().Add(stockSnapshot);
    }
}