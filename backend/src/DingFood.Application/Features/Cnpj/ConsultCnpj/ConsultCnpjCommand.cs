using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cnpj.ConsultCnpj;

/// <summary>
/// Consulta um CNPJ respeitando o cache. É um Command (e não Query) porque grava/atualiza
/// o snapshot no banco, e portanto precisa do IUnitOfWork do BaseCommandHandler.
/// </summary>
/// <param name="TaxId">CNPJ com ou sem pontuação.</param>
/// <param name="ForceRefresh">Ignora o cache e força nova chamada à CNPJá.</param>
public sealed record ConsultCnpjCommand(string TaxId, bool ForceRefresh = false) : ICommand<CnpjResponse>;
