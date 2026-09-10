using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

// Fase 9b — confirma o código de retirada informado pelo cliente/entregador (POST
// orders/{id}/validatePickupCode). Retorna bool: true = código confere, false = código errado
// (não é erro de requisição — a equipe pode pedir pro cliente tentar de novo).
public sealed record ValidateIfoodPickupCodeCommand(long IfoodOrderId, string Code) : ICommand<bool>;
