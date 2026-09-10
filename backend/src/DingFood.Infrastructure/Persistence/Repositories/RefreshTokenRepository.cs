using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    // Tracked — o token e revogado na renovacao.
    public async Task<RefreshToken?> GetByTokenForUpdateAsync(string token, CancellationToken cancellationToken = default)
        => await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

    public async Task AddAsync(RefreshToken entity, CancellationToken cancellationToken = default)
        => await context.RefreshTokens.AddAsync(entity, cancellationToken);
}
