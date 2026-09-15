using DingFood.Application.Abstractions.Integrations.ViaCep;
using Microsoft.Extensions.Options;

namespace DingFood.Infrastructure.Integrations.ViaCep;

internal sealed class ViaCepOptionsProvider(IOptions<ViaCepSettings> options) : IViaCepOptions
{
    public int CacheTtlDays => options.Value.CacheTtlDays;
}
