namespace DingFood.Infrastructure.Integrations.Cnpja;

public sealed class CnpjaSettings
{
    public const string SectionName = "Cnpja";

    /// <summary>API pública (sem autenticação). Para a API Comercial: https://api.cnpja.com</summary>
    public string BaseUrl { get; set; } = "https://open.cnpja.com";

    public int TimeoutSeconds { get; set; } = 15;

    /// <summary>Dias que um snapshot é considerado válido antes de reconsultar. 0 desliga o cache.</summary>
    public int CacheTtlDays { get; set; } = 30;

    /// <summary>
    /// Vazio na API pública. Preenchido, é enviado no header Authorization — o que já deixa
    /// a integração pronta para a API Comercial sem mudança de código.
    /// </summary>
    public string? ApiKey { get; set; }
}
