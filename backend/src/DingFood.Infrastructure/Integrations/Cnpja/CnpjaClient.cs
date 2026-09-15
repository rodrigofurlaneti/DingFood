using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using DingFood.Application.Abstractions.Integrations.Cnpja;
using DingFood.Domain.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DingFood.Infrastructure.Integrations.Cnpja;

/// <summary>
/// Client HTTP da API pública da CNPJá — GET {BaseUrl}/office/{cnpj}.
///
/// Limite da API pública: 5 consultas por minuto por IP. A primeira linha de defesa é o cache
/// no banco (ConsultCnpjCommandHandler); aqui tratamos o 429 com uma única retentativa que
/// respeita o header Retry-After, e além disso devolvemos o erro tipado para o handler decidir
/// se serve um dado antigo.
/// </summary>
public sealed class CnpjaClient : ICnpjaClient
{
    private const int MaxAttempts = 2;
    private static readonly TimeSpan DefaultRetryDelay = TimeSpan.FromSeconds(12);
    private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(20);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;
    private readonly ILogger<CnpjaClient> _logger;

    public CnpjaClient(HttpClient httpClient, IOptions<CnpjaSettings> options, ILogger<CnpjaClient> logger)
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

        // A API Comercial autentica com a chave crua no header Authorization (sem esquema Bearer).
        if (!string.IsNullOrWhiteSpace(settings.ApiKey) && _http.DefaultRequestHeaders.Authorization is null)
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(settings.ApiKey);
    }

    public async Task<Result<CnpjaOfficeResult>> GetOfficeAsync(
        string taxId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(taxId) || taxId.Length != 14 || !taxId.All(char.IsDigit))
        {
            return Result.Failure<CnpjaOfficeResult>(
                new Error("Cnpj.Invalid", "O CNPJ informado é inválido."));
        }

        var path = $"office/{taxId}";

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                using var response = await _http.GetAsync(path, cancellationToken);

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var delay = ResolveRetryDelay(response);

                    if (attempt < MaxAttempts && delay <= MaxRetryDelay)
                    {
                        _logger.LogWarning(
                            "CNPJá respondeu 429 para {TaxId}. Nova tentativa em {Delay}s.",
                            taxId, delay.TotalSeconds);

                        await Task.Delay(delay, cancellationToken);
                        continue;
                    }

                    return Result.Failure<CnpjaOfficeResult>(new Error(
                        "Cnpja.RateLimited",
                        "Limite de consultas da CNPJá atingido (5 por minuto). Tente novamente em instantes."));
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Failure<CnpjaOfficeResult>(new Error(
                        "Cnpj.NotFound",
                        $"CNPJ {taxId} não encontrado na base da Receita Federal."));
                }

                var body = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "CNPJá retornou HTTP {Status} para {TaxId}: {Body}",
                        (int)response.StatusCode, taxId, Truncate(body, 500));

                    return Result.Failure<CnpjaOfficeResult>(new Error(
                        "Cnpja.Unavailable",
                        $"A consulta de CNPJ falhou (HTTP {(int)response.StatusCode})."));
                }

                var payload = JsonSerializer.Deserialize<CnpjaOfficeResponse>(body, JsonOptions);

                if (payload is null || string.IsNullOrWhiteSpace(payload.TaxId))
                {
                    _logger.LogWarning("CNPJá devolveu payload vazio para {TaxId}.", taxId);

                    return Result.Failure<CnpjaOfficeResult>(new Error(
                        "Cnpja.Unavailable", "A CNPJá devolveu uma resposta vazia."));
                }

                return Result.Success(new CnpjaOfficeResult(payload, body));
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Timeout ao consultar {TaxId} na CNPJá.", taxId);

                return Result.Failure<CnpjaOfficeResult>(new Error(
                    "Cnpja.Timeout", "A consulta de CNPJ excedeu o tempo limite."));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Falha de rede ao consultar {TaxId} na CNPJá.", taxId);

                return Result.Failure<CnpjaOfficeResult>(new Error(
                    "Cnpja.Unavailable", "Não foi possível contatar o serviço de consulta de CNPJ."));
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Resposta da CNPJá em formato inesperado para {TaxId}.", taxId);

                return Result.Failure<CnpjaOfficeResult>(new Error(
                    "Cnpja.Unavailable", "A resposta do serviço de consulta de CNPJ está em formato inesperado."));
            }
        }

        return Result.Failure<CnpjaOfficeResult>(new Error(
            "Cnpja.RateLimited",
            "Limite de consultas da CNPJá atingido (5 por minuto). Tente novamente em instantes."));
    }

    private static TimeSpan ResolveRetryDelay(HttpResponseMessage response)
    {
        var retryAfter = response.Headers.RetryAfter;

        if (retryAfter?.Delta is { } delta && delta > TimeSpan.Zero)
            return delta;

        if (retryAfter?.Date is { } date)
        {
            var wait = date - DateTimeOffset.UtcNow;
            if (wait > TimeSpan.Zero)
                return wait;
        }

        return DefaultRetryDelay;
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }
}
