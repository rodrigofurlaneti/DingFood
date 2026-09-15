using System.Text.Json.Serialization;

namespace DingFood.Application.Abstractions.Integrations.ViaCep;

/// <summary>
/// Contrato do ViaCEP: GET https://viacep.com.br/ws/{cep}/json/
/// Documentação: https://viacep.com.br/
///
/// Atenção a duas particularidades do serviço, tratadas no ViaCepClient:
/// 1. CEP com formato válido mas inexistente devolve HTTP 200 com {"erro": true} — e o tipo
///    desse campo variou entre boolean e string ("true") ao longo das versões do serviço.
/// 2. Campos sem valor vêm como string vazia, não como null.
/// </summary>
public sealed record ViaCepAddressResponse
{
    /// <summary>Vem formatado da origem: "01001-000".</summary>
    [JsonPropertyName("cep")]
    public string? Cep { get; init; }

    [JsonPropertyName("logradouro")]
    public string? Logradouro { get; init; }

    [JsonPropertyName("complemento")]
    public string? Complemento { get; init; }

    [JsonPropertyName("unidade")]
    public string? Unidade { get; init; }

    [JsonPropertyName("bairro")]
    public string? Bairro { get; init; }

    /// <summary>Nome do município.</summary>
    [JsonPropertyName("localidade")]
    public string? Localidade { get; init; }

    [JsonPropertyName("uf")]
    public string? Uf { get; init; }

    [JsonPropertyName("estado")]
    public string? Estado { get; init; }

    [JsonPropertyName("regiao")]
    public string? Regiao { get; init; }

    /// <summary>Código IBGE do município, como texto na origem ("3550308").</summary>
    [JsonPropertyName("ibge")]
    public string? Ibge { get; init; }

    /// <summary>GIA/ICMS — preenchido somente para São Paulo.</summary>
    [JsonPropertyName("gia")]
    public string? Gia { get; init; }

    [JsonPropertyName("ddd")]
    public string? Ddd { get; init; }

    [JsonPropertyName("siafi")]
    public string? Siafi { get; init; }
}

/// <summary>
/// Resposta junto com o corpo HTTP original, preservado para gravação em CepQuery.RawJson.
/// </summary>
public sealed record ViaCepAddressResult(ViaCepAddressResponse Payload, string RawJson);
