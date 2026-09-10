using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

public sealed record AcknowledgeIfoodOperationalAlertCommand(long CompanyId, Guid AlertId) : ICommand;
