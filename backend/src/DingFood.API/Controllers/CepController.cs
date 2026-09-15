using DingFood.Application.Features.Cep.ConsultCep;
using DingFood.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DingFood.API.Controllers;

/// <summary>
/// Consulta de CEP na base dos Correios (via ViaCEP), com cache no banco.
/// Rota base: api/cep (convenção [Route("api/[controller]")] herdada de ApiController).
/// </summary>
public sealed class CepController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    /// <summary>
    /// GET api/cep/01001000 — devolve do cache quando o snapshot ainda está dentro do TTL.
    /// Aceita CEP com ou sem hífen.
    /// </summary>
    [HttpGet("{cep}")]
    public Task<IActionResult> GetByCep(string cep, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CepController), nameof(GetByCep), async () =>
        {
            var result = await Mediator.Send(new ConsultCepCommand(cep), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    /// <summary>
    /// POST api/cep/01001000/refresh — ignora o cache e força nova consulta ao ViaCEP.
    /// Restrito a gestores: o ViaCEP bloqueia acesso por volume, então forçar consulta
    /// não deve ficar aberto a qualquer visitante da tela de cadastro.
    /// </summary>
    [Authorize(Roles = ManagerRoles)]
    [HttpPost("{cep}/refresh")]
    public Task<IActionResult> Refresh(string cep, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CepController), nameof(Refresh), async () =>
        {
            var result = await Mediator.Send(new ConsultCepCommand(cep, ForceRefresh: true), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });
}
