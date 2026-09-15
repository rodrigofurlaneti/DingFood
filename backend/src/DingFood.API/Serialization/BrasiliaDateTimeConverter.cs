using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using DingFood.Domain.Constants;

namespace DingFood.API.Serialization;

/// <summary>
/// Serializa datas em horário de Brasília com o offset explícito (ex.: "2026-09-15T10:30:00.0000000-03:00").
///
/// Substitui o antigo UtcDateTimeConverter, que carimbava 'Z' em toda data vinda do banco.
/// Aquilo partia da premissa de que o banco guardava UTC — mas as entidades gravam
/// `DateTime.Now`, que é horário de Brasília. O resultado era um instante local rotulado como
/// UTC, e o navegador exibia tudo 3 horas adiantado.
///
/// Com o offset explícito a data fica sem ambiguidade: `new Date("...-03:00")` no JavaScript
/// resolve o instante correto em qualquer fuso do cliente, sem depender de o navegador estar
/// no Brasil.
///
/// Regra de leitura do Kind na escrita:
///   Unspecified -> já está em Brasília (é o que o EF devolve do banco)
///   Utc         -> converte para Brasília
///   Local       -> converte do fuso do servidor para Brasília
/// </summary>
internal sealed class BrasiliaDateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd'T'HH:mm:ss.fffffffK";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString()
            ?? throw new JsonException("Data esperada, valor nulo recebido.");

        // RoundtripKind preserva a informação de offset: 'Z' vira Utc, "-03:00" vira Local,
        // e sem sufixo vira Unspecified.
        var parsed = DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

        return parsed.Kind switch
        {
            DateTimeKind.Utc => BrazilTimeZone.FromUtc(parsed),
            DateTimeKind.Local => TimeZoneInfo.ConvertTime(parsed, TimeZoneInfo.Local, BrazilTimeZone.Brasilia),
            _ => parsed, // já veio em horário de Brasília
        };
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var brasilia = value.Kind switch
        {
            DateTimeKind.Utc => BrazilTimeZone.FromUtc(value),
            DateTimeKind.Local => TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Local, BrazilTimeZone.Brasilia),
            _ => value,
        };

        var offset = new DateTimeOffset(
            DateTime.SpecifyKind(brasilia, DateTimeKind.Unspecified),
            BrazilTimeZone.OffsetAt(brasilia));

        writer.WriteStringValue(offset.ToString(Format, CultureInfo.InvariantCulture));
    }
}
