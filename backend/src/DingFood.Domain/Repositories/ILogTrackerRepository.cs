using DingFood.Domain.Entities;
namespace DingFood.Domain.Repositories
{
    public interface ILogTrackerRepository
    {
        Task AddAsync(LogTracker entity, CancellationToken cancellationToken = default);
    }
}
