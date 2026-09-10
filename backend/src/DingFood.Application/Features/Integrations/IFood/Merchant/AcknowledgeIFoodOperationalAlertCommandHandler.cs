using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

internal sealed class AcknowledgeIfoodOperationalAlertCommandHandler(
    IIfoodOperationalAlertStore alertStore,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<AcknowledgeIfoodOperationalAlertCommand>(logRepository, unitOfWork)
{
    public override async Task<Result> Handle(AcknowledgeIfoodOperationalAlertCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(AcknowledgeIfoodOperationalAlertCommandHandler),
            nameof(Handle),
            null,
            (_) =>
            {
                // Idempotente de propósito: reconhecer um alerta que já sumiu (por já ter sido
                // reconhecido em outra aba, ou por ter estourado o limite de MaxPerCompany) não é
                // erro — o resultado desejado ("esse alerta não aparece mais") já está garantido.
                alertStore.Acknowledge(request.CompanyId, request.AlertId);
                return Task.FromResult(Result.Success());
            });
    }
}
