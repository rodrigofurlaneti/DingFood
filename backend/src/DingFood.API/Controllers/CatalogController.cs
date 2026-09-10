using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DingFood.Application.Features.Catalog.GetMenu;
using DingFood.Application.Features.Promotions.GetActive;
using DingFood.Domain.Repositories;

namespace DingFood.API.Controllers;

[Authorize]
public sealed class CatalogController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    [HttpGet("promotions/active/branch/{branchId:long}")]
    public Task<IActionResult> GetActivePromotions(long branchId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CatalogController), nameof(GetActivePromotions), async () =>
        {
            var result = await Mediator.Send(new GetActivePromotionsQuery(branchId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpGet("menu/company/{companyId:long}")]
    public Task<IActionResult> GetMenu(long companyId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CatalogController), nameof(GetMenu), async () =>
        {
            var result = await Mediator.Send(new GetMenuQuery(companyId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });
}