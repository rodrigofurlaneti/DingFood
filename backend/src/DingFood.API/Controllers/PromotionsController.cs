using System.Diagnostics;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DingFood.Application.Features.Promotions.Create;
using DingFood.Application.Features.Promotions.Deactivate;
using DingFood.Application.Features.Promotions.GetByBranch;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.API.Controllers;

public sealed class PromotionsController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    [HttpGet("branch/{branchId:long}")]
    public Task<IActionResult> GetByBranch(long branchId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PromotionsController), nameof(GetByBranch), async () =>
        {
            var result = await Mediator.Send(new GetPromotionsByBranchQuery(branchId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreatePromotionCommand command, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PromotionsController), nameof(Create), async () =>
        {
            var result = await Mediator.Send(command, ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [Authorize(Roles = ManagerRoles)]
    [HttpPut("{id:long}/deactivate")]
    public Task<IActionResult> Deactivate(long id, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PromotionsController), nameof(Deactivate), async () =>
        {
            var result = await Mediator.Send(new DeactivatePromotionCommand(id), ct);
            return result.IsFailure ? HandleFailure(result) : NoContent();
        });
}