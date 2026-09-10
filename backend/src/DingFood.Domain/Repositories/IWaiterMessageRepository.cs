using DingFood.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace DingFood.Domain.Repositories
{
    public interface IWaiterMessageRepository
    {
        Task<WaiterMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<WaiterMessage>> GetByBranchIdAsync(long branchId, CancellationToken cancellationToken = default);
        Task AddAsync(WaiterMessage entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(WaiterMessage entity, CancellationToken cancellationToken = default);
    }
}
