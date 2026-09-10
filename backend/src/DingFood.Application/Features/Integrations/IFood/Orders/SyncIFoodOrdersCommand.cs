using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

// Um ciclo de polling para UMA empresa — disparado pelo IfoodOrderPollingBackgroundService a
// cada 30s para cada empresa com integração habilitada. Não é chamado pela API/frontend.
public sealed record SyncIfoodOrdersCommand(long CompanyId,
    IReadOnlyCollection<DingFood.Application.Abstractions.Integrations.Ifood.IfoodPollingEvent>? ReceivedEvents = null) : ICommand;
