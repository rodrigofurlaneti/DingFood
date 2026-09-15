namespace DingFood.Domain.Constants;

/// <summary>
/// Fuso oficial da aplicação: horário de Brasília.
///
/// O sistema grava e exibe tudo em horário de Brasília — não em UTC e não no fuso da máquina.
/// `DateTime.Now` sozinho NÃO garante isso: ele devolve a hora local do servidor, que em
/// container Linux com TZ padrão é UTC. Por isso duas coisas se somam:
///
/// 1. O Dockerfile define TZ=America/Sao_Paulo, alinhando `DateTime.Now` das entidades.
/// 2. Quem precisa de garantia independente da máquina usa este tipo (ou o TimeProvider
///    injetado, que aponta para cá).
///
/// O Brasil aboliu o horário de verão em 2019, mas a conversão é feita via TimeZoneInfo — e não
/// com "-3" fixo — para que datas históricas e uma eventual volta do DST continuem corretas.
/// </summary>
public static class BrazilTimeZone
{
    private const string IanaId = "America/Sao_Paulo";
    private const string WindowsId = "E. South America Standard Time";

    /// <summary>Fuso de Brasília, resolvido uma única vez.</summary>
    public static TimeZoneInfo Brasilia { get; } = Resolve();

    /// <summary>Agora, em horário de Brasília, independente do fuso do servidor.</summary>
    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Brasilia);

    /// <summary>Converte um instante UTC para horário de Brasília (Kind = Unspecified).</summary>
    public static DateTime FromUtc(DateTime utc)
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Brasilia);

    /// <summary>Offset vigente na data informada — -03:00 hoje, -02:00 em datas de horário de verão.</summary>
    public static TimeSpan OffsetAt(DateTime brasiliaLocal)
        => Brasilia.GetUtcOffset(DateTime.SpecifyKind(brasiliaLocal, DateTimeKind.Unspecified));

    private static TimeZoneInfo Resolve()
    {
        // .NET 6+ aceita id IANA também no Windows (via ICU), mas a instalação pode estar em
        // modo globalization-invariant — daí o fallback para o id do Windows e, por último,
        // um fuso fixo, para a aplicação nunca deixar de subir por causa disso.
        foreach (var id in new[] { IanaId, WindowsId })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException) { }
            catch (InvalidTimeZoneException) { }
        }

        return TimeZoneInfo.CreateCustomTimeZone(
            IanaId, TimeSpan.FromHours(-3), "Horário de Brasília", "Horário de Brasília");
    }
}
