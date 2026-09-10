using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class AccessLogRepository(AppDbContext context) : IAccessLogRepository
{
    public async Task AddAsync(AccessLog entity, CancellationToken cancellationToken = default)
        => await context.AccessLogs.AddAsync(entity, cancellationToken);
}
