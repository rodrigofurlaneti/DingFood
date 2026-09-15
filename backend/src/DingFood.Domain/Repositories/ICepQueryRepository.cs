using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICepQueryRepository
{
    /// <summary>Leitura sem tracking, para servir o cache.</summary>
    Task<CepQuery?> GetByCepAsync(string cep, CancellationToken cancellationToken = default);

    /// <summary>Leitura com tracking, para permitir <see cref="CepQuery.Refresh"/>.</summary>
    Task<CepQuery?> GetByCepForUpdateAsync(string cep, CancellationToken cancellationToken = default);

    /// <summary>Últimas consultas realizadas, da mais recente para a mais antiga.</summary>
    Task<IReadOnlyCollection<CepQuery>> GetRecentAsync(int take, CancellationToken cancellationToken = default);

    Task AddAsync(CepQuery entity, CancellationToken cancellationToken = default);
}
