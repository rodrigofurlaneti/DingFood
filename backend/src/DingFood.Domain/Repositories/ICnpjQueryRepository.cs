using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICnpjQueryRepository
{
    /// <summary>Leitura sem tracking, para servir o cache.</summary>
    Task<CnpjQuery?> GetByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);

    /// <summary>Leitura com tracking, para permitir <see cref="CnpjQuery.Refresh"/>.</summary>
    Task<CnpjQuery?> GetByTaxIdForUpdateAsync(string taxId, CancellationToken cancellationToken = default);

    /// <summary>Últimas consultas realizadas, ordenadas da mais recente para a mais antiga.</summary>
    Task<IReadOnlyCollection<CnpjQuery>> GetRecentAsync(int take, CancellationToken cancellationToken = default);

    Task AddAsync(CnpjQuery entity, CancellationToken cancellationToken = default);
}
