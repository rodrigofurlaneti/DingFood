using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICustomerRefreshTokenRepository
{
    // Tracked — o token e revogado na renovacao.
    Task<CustomerRefreshToken?> GetByTokenForUpdateAsync(string token, CancellationToken cancellationToken = default);
    Task AddAsync(CustomerRefreshToken entity, CancellationToken cancellationToken = default);
}
