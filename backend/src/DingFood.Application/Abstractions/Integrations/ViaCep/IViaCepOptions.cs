namespace DingFood.Application.Abstractions.Integrations.ViaCep;

/// <summary>
/// Expõe à Application as opções de cache da integração sem acoplar a camada a
/// IOptions&lt;ViaCepSettings&gt;, que vive na Infrastructure.
/// </summary>
public interface IViaCepOptions
{
    /// <summary>Janela de validade do cache em dias. Zero ou negativo desliga o cache.</summary>
    int CacheTtlDays { get; }
}
