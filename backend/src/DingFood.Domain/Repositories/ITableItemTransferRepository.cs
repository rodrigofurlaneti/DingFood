using DingFood.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace DingFood.Domain.Repositories
{
    public interface ITableItemTransferRepository
    {
        Task AddAsync(TableItemTransfer entity, CancellationToken cancellationToken = default);
    }
}