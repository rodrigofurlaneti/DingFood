using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IRefreshTokenRepository
{
    // Tracked — o token e revogado na renovacao.
    Task<RefreshToken?> GetByTokenForUpdateAsync(string token, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken entity, CancellationToken cancellationToken = default);
}
