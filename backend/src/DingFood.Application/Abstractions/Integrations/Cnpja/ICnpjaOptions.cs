namespace DingFood.Application.Abstractions.Integrations.Cnpja;

/// <summary>
/// Expõe à Application as opções de cache da integração sem obrigar a camada a conhecer
/// IOptions&lt;CnpjaSettings&gt;, que vive na Infrastructure.
/// </summary>
public interface ICnpjaOptions
{
    /// <summary>Janela de validade do cache em dias. Zero ou negativo desliga o cache.</summary>
    int CacheTtlDays { get; }
}
