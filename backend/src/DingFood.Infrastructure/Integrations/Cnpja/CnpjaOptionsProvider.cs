using DingFood.Application.Abstractions.Integrations.Cnpja;
using Microsoft.Extensions.Options;

namespace DingFood.Infrastructure.Integrations.Cnpja;

internal sealed class CnpjaOptionsProvider(IOptions<CnpjaSettings> options) : ICnpjaOptions
{
    public int CacheTtlDays => options.Value.CacheTtlDays;
}
