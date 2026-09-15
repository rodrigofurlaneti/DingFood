using DingFood.Application.Features.Cnpj.ConsultCnpj;
using DingFood.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DingFood.API.Controllers;

/// <summary>
/// Consulta de CNPJ na base pública da Receita Federal (via CNPJá), com cache no banco.
/// Rota base: api/cnpj (convenção [Route("api/[controller]")] herdada de ApiController).
/// </summary>
public sealed class CnpjController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    /// <summary>
    /// GET api/cnpj/14486046000177 — devolve do cache quando o snapshot ainda está dentro do TTL.
    /// Aceita CNPJ com ou sem pontuação.
    /// </summary>
    [HttpGet("{taxId}")]
    public Task<IActionResult> GetByTaxId(string taxId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CnpjController), nameof(GetByTaxId), async () =>
        {
            var result = await Mediator.Send(new ConsultCnpjCommand(taxId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    /// <summary>
    /// POST api/cnpj/14486046000177/refresh — ignora o cache e força nova consulta à CNPJá.
    /// Restrito a gestores porque consome a cota de 5 requisições/minuto da API pública.
    /// </summary>
    [HttpPost("{taxId}/refresh")]
    public Task<IActionResult> Refresh(string taxId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CnpjController), nameof(Refresh), async () =>
        {
            var result = await Mediator.Send(new ConsultCnpjCommand(taxId, ForceRefresh: true), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });
}
