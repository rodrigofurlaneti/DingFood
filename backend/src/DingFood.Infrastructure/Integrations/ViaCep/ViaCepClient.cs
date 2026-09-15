using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using DingFood.Application.Abstractions.Integrations.ViaCep;
using DingFood.Domain.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DingFood.Infrastructure.Integrations.ViaCep;

/// <summary>
/// Client HTTP do ViaCEP — GET {BaseUrl}/ws/{cep}/json/
///
/// O serviço é gratuito e sem autenticação, mas avisa: "uso massivo para validação de bases de
/// dados locais poderá automaticamente bloquear seu acesso por tempo indeterminado". A defesa é
/// o cache no banco (ConsultCepCommandHandler) — este client não faz retentativa agressiva.
/// </summary>
public sealed class ViaCepClient : IViaCepClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;
    private readonly ILogger<ViaCepClient> _logger;

    public ViaCepClient(HttpClient httpClient, IOptions<ViaCepSettings> options, ILogger<ViaCepClient> logger)
    {
        _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;

        var settings = options.Value;

        if (_http.BaseAddress is null && !string.IsNullOrWhiteSpace(settings.BaseUrl))
            _http.BaseAddress = new Uri(settings.BaseUrl.TrimEnd('/') + "/");

        if (settings.TimeoutSeconds > 0)
            _http.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);

        if (_http.DefaultRequestHeaders.Accept.Count == 0)
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (_http.DefaultRequestHeaders.UserAgent.Count == 0)
            _http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DingFood", "1.0"));
    }

    public async Task<Result<ViaCepAddressResult>> GetAddressAsync(
        string cep, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !cep.All(char.IsDigit))
        {
            return Result.Failure<ViaCepAddressResult>(
                new Error("Cep.Invalid", "O CEP informado é inválido."));
        }

        try
        {
            using var response = await _http.GetAsync($"ws/{cep}/json/", cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // O ViaCEP responde 400 para CEP malformado. Como validamos antes, chegar aqui
                // significa divergência de formato, não erro do usuário.
                _logger.LogWarning("ViaCEP recusou o formato do CEP {Cep}.", cep);

                return Result.Failure<ViaCepAddressResult>(
                    new Error("Cep.Invalid", "O CEP informado é inválido."));
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                _logger.LogWarning(
                    "ViaCEP respondeu {Status} para {Cep} — possível bloqueio por volume de acesso.",
                    (int)response.StatusCode, cep);

                return Result.Failure<ViaCepAddressResult>(new Error(
                    "ViaCep.RateLimited",
                    "O serviço de consulta de CEP recusou a requisição por excesso de acessos. Tente novamente em instantes."));
            }

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "ViaCEP retornou HTTP {Status} para {Cep}: {Body}",
                    (int)response.StatusCode, cep, Truncate(body, 300));

                return Result.Failure<ViaCepAddressResult>(new Error(
                    "ViaCep.Unavailable",
                    $"A consulta de CEP falhou (HTTP {(int)response.StatusCode})."));
            }

            // CEP com formato válido mas inexistente volta como HTTP 200 + {"erro": ...}.
            if (HasErrorFlag(body))
            {
                return Result.Failure<ViaCepAddressResult>(new Error(
                    "Cep.NotFound", $"CEP {cep} não encontrado na base dos Correios."));
            }

            var payload = JsonSerializer.Deserialize<ViaCepAddressResponse>(body, JsonOptions);

            if (payload is null || string.IsNullOrWhiteSpace(payload.Cep))
            {
                _logger.LogWarning("ViaCEP devolveu payload vazio para {Cep}.", cep);

                return Result.Failure<ViaCepAddressResult>(new Error(
                    "ViaCep.Unavailable", "O serviço de consulta de CEP devolveu uma resposta vazia."));
            }

            return Result.Success(new ViaCepAddressResult(payload, body));
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Timeout ao consultar o CEP {Cep} no ViaCEP.", cep);

            return Result.Failure<ViaCepAddressResult>(new Error(
                "ViaCep.Timeout", "A consulta de CEP excedeu o tempo limite."));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Falha de rede ao consultar o CEP {Cep} no ViaCEP.", cep);

            return Result.Failure<ViaCepAddressResult>(new Error(
                "ViaCep.Unavailable", "Não foi possível contatar o serviço de consulta de CEP."));
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Resposta do ViaCEP em formato inesperado para o CEP {Cep}.", cep);

            return Result.Failure<ViaCepAddressResult>(new Error(
                "ViaCep.Unavailable", "A resposta do serviço de consulta de CEP está em formato inesperado."));
        }
    }

    /// <summary>
    /// Detecta o campo "erro" do ViaCEP. O tipo desse campo variou entre versões do serviço —
    /// já veio como boolean (true) e como string ("true") — então a checagem aceita os dois em
    /// vez de depender de um único formato de desserialização.
    /// </summary>
    private static bool HasErrorFlag(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return false;

        using var document = JsonDocument.Parse(body);

        if (document.RootElement.ValueKind != JsonValueKind.Object)
            return false;

        if (!document.RootElement.TryGetProperty("erro", out var erro))
            return false;

        return erro.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.String => bool.TryParse(erro.GetString(), out var parsed) && parsed,
            _ => false
        };
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }
}
