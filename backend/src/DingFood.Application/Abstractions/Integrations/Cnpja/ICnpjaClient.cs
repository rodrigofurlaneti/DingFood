using DingFood.Domain.Primitives;

namespace DingFood.Application.Abstractions.Integrations.Cnpja;

public interface ICnpjaClient
{
    /// <summary>
    /// Consulta um estabelecimento na API pública da CNPJá.
    /// Nunca lança por falha remota — devolve <see cref="Result"/> com um dos códigos:
    /// "Cnpj.Invalid", "Cnpj.NotFound", "Cnpja.RateLimited", "Cnpja.Timeout", "Cnpja.Unavailable".
    /// </summary>
    /// <param name="taxId">CNPJ com 14 dígitos, sem pontuação.</param>
    Task<Result<CnpjaOfficeResult>> GetOfficeAsync(string taxId, CancellationToken cancellationToken = default);
}
