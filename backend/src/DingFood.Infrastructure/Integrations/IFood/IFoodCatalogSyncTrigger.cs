using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Features.Integrations.Ifood.Catalog;

namespace DingFood.Infrastructure.Integrations.Ifood;

internal sealed class IfoodCatalogSyncTrigger(IServiceScopeFactory scopeFactory, DingFood.Application.Abstractions.Tenancy.ICurrentTenantService? tenant = null) : IIfoodCatalogSyncTrigger
{
    public void TriggerCompanySync(long companyId)
    {
        var brandId = tenant?.BrandId;
        using var flow = ExecutionContext.SuppressFlow();
        _ = Task.Run(async () =>
        {
            try
            {
                // Escopo próprio (não o da requisição HTTP que disparou isso) — a sincronização
                // faz várias chamadas HTTP pro Ifood e pode demorar mais que o tempo de vida do
                // escopo original. Mesmo padrão usado por IfoodOrderPollingBackgroundService.
                using var scope = scopeFactory.CreateScope();
                scope.ServiceProvider.GetService<DingFood.Infrastructure.Tenancy.CurrentTenantService>()?.SetBackgroundCompany(companyId, brandId);
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SyncIfoodCatalogCommand(companyId));
            }
            catch
            {
                // Best-effort: falha aqui não deve derrubar o fluxo que criou/editou o produto —
                // o botão "Sincronizar agora" na tela de integrações cobre o reenvio manual.
            }
        });
    }
}
