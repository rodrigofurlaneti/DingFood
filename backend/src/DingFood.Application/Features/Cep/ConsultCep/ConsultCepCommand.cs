using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cep.ConsultCep;

/// <summary>
/// Consulta um CEP respeitando o cache. É um Command (e não Query) porque grava/atualiza
/// o snapshot no banco, e portanto precisa do IUnitOfWork do BaseCommandHandler.
/// </summary>
/// <param name="Cep">CEP com ou sem hífen.</param>
/// <param name="ForceRefresh">Ignora o cache e força nova consulta ao ViaCEP.</param>
public sealed record ConsultCepCommand(string Cep, bool ForceRefresh = false) : ICommand<CepResponse>;
