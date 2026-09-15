using DingFood.Domain.Constants;

namespace DingFood.Infrastructure.Time;

/// <summary>
/// TimeProvider da aplicação. `GetUtcNow()` continua devolvendo UTC (contrato da classe base),
/// mas `LocalTimeZone` aponta explicitamente para o horário de Brasília — então
/// `GetLocalNow()` devolve Brasília mesmo que o servidor esteja em UTC.
///
/// É o que garante o fuso para o código que recebe TimeProvider por injeção. As entidades, que
/// usam `DateTime.Now` direto, dependem do TZ do processo (definido no Dockerfile e no serviço
/// systemd da VM) — ver DingFood.Domain.Constants.BrazilTimeZone.
/// </summary>
public sealed class TimeProviderCustom : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;

    public override TimeZoneInfo LocalTimeZone => BrazilTimeZone.Brasilia;
}
