using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DingFood.Application.Features.Integrations.Ifood.Orders;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Integrations.Ifood;

/// <summary>
/// Loop de polling do módulo Order/Events do Ifood — a cada 30s,
/// para cada empresa com integração habilitada, dispara um ciclo de sincronização
/// (SyncIfoodOrdersCommand). Um BackgroundService é singleton — cria um scope de DI por ciclo
/// pra resolver serviços scoped (DbContext, repositórios, MediatR).
/// </summary>
internal sealed class IfoodOrderPollingBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<IfoodOrderPollingBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Pequeno atraso inicial pra deixar a API terminar de subir antes do primeiro ciclo.
        try { await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); }
        catch (OperationCanceledException) { return; }

        using var timer = new PeriodicTimer(PollInterval);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunCycleAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ciclo de polling do Ifood falhou inesperadamente.");
            }

            try { if (!await timer.WaitForNextTickAsync(stoppingToken)) break; }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task RunCycleAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var settingRepository = scope.ServiceProvider.GetRequiredService<IIfoodIntegrationSettingRepository>();

        var companyIds = await settingRepository.GetEnabledCompanyIdsAsync(stoppingToken);
        await Parallel.ForEachAsync(companyIds.Distinct(), new ParallelOptions
        {
            MaxDegreeOfParallelism = 4, CancellationToken = stoppingToken
        }, async (companyId, ct) =>
        {
            try
            {
                using var companyScope = serviceProvider.CreateScope();
                companyScope.ServiceProvider.GetRequiredService<DingFood.Infrastructure.Tenancy.CurrentTenantService>()
                    .SetBackgroundCompany(companyId);
                var mediator = companyScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SyncIfoodOrdersCommand(companyId), ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao sincronizar pedidos Ifood da empresa {CompanyId}.", companyId);
            }
        });
    }
}
