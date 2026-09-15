namespace DingFood.Infrastructure.Integrations.ViaCep;

public sealed class ViaCepSettings
{
    public const string SectionName = "ViaCep";

    public string BaseUrl { get; set; } = "https://viacep.com.br";

    public int TimeoutSeconds { get; set; } = 10;

    /// <summary>
    /// Dias que um snapshot é considerado válido. Endereço de CEP muda muito pouco e o ViaCEP
    /// bloqueia acesso por uso massivo, então o padrão é bem mais longo que o do CNPJ.
    /// 0 desliga o cache.
    /// </summary>
    public int CacheTtlDays { get; set; } = 180;
}
