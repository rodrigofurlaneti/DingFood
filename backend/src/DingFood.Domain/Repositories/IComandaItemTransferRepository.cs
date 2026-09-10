using DingFood.Domain.Entities;
namespace DingFood.Domain.Repositories
{
    public interface IComandaItemTransferRepository
    {
        Task AddAsync(ComandaItemTransfer entity, CancellationToken cancellationToken = default);
    }
}
