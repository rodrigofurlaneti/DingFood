using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
namespace DingFood.Infrastructure.Persistence.Repositories
{
    internal sealed class ComandaItemTransferRepository(AppDbContext context) : IComandaItemTransferRepository
    {
        public async Task AddAsync(ComandaItemTransfer entity, CancellationToken cancellationToken = default)
            => await context.Set<ComandaItemTransfer>().AddAsync(entity, cancellationToken);
    }
}
