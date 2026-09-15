using DingFood.Domain.Primitives;

namespace DingFood.Application.Abstractions.Integrations.ViaCep;

public interface IViaCepClient
{
    /// <summary>
    /// Consulta um CEP no ViaCEP.
    /// Nunca lança por falha remota — devolve <see cref="Result"/> com um dos códigos:
    /// "Cep.Invalid", "Cep.NotFound", "ViaCep.RateLimited", "ViaCep.Timeout", "ViaCep.Unavailable".
    /// </summary>
    /// <param name="cep">CEP com 8 dígitos, sem pontuação.</param>
    Task<Result<ViaCepAddressResult>> GetAddressAsync(string cep, CancellationToken cancellationToken = default);
}
