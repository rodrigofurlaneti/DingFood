using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DingFood.Application.Features.Catalog.ActivateCategory;
using DingFood.Application.Features.Catalog.CreateCategory;
using DingFood.Application.Features.Catalog.DeactivateCategory;
using DingFood.Application.Features.Catalog.GetCategories;
using DingFood.Application.Features.Catalog.GetCategoriesForManagement;
using DingFood.Application.Features.Catalog.GetCategoryById;
using DingFood.Application.Features.Catalog.UpdateCategory;
using DingFood.Domain.Repositories;

namespace DingFood.API.Controllers;

public sealed class CategoriesController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    [Authorize(Policy = "AppUser")]
    [HttpGet("company/{companyId:long}")]
    public Task<IActionResult> GetByCompany(long companyId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(GetByCompany), async () =>
        {
            var result = await Mediator.Send(new GetCategoriesQuery(companyId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpGet("{id:long}")]
    public Task<IActionResult> GetById(long id, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(GetById), async () =>
        {
            var result = await Mediator.Send(new GetCategoryByIdQuery(id), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpGet("company/{companyId:long}/management")]
    public Task<IActionResult> GetForManagement(long companyId, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(GetForManagement), async () =>
        {
            var result = await Mediator.Send(new GetCategoriesForManagementQuery(companyId), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [Authorize(Roles = ManagerRoles)]
    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(Create), async () =>
        {
            var result = await Mediator.Send(command, ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [Authorize(Roles = ManagerRoles)]
    [HttpPut("{id:long}")]
    public Task<IActionResult> Update(long id, [FromBody] UpdateCategoryRequest request, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(Update), async () =>
        {
            var result = await Mediator.Send(new UpdateCategoryCommand(id, request.Name, request.DisplayOrder), ct);
            return result.IsFailure ? HandleFailure(result) : NoContent();
        });

    [Authorize(Roles = ManagerRoles)]
    [HttpPut("{id:long}/deactivate")]
    public Task<IActionResult> Deactivate(long id, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(Deactivate), async () =>
        {
            var result = await Mediator.Send(new DeactivateCategoryCommand(id), ct);
            return result.IsFailure ? HandleFailure(result) : NoContent();
        });

    [Authorize(Roles = ManagerRoles)]
    [HttpPut("{id:long}/activate")]
    public Task<IActionResult> Activate(long id, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CategoriesController), nameof(Activate), async () =>
        {
            var result = await Mediator.Send(new ActivateCategoryCommand(id), ct);
            return result.IsFailure ? HandleFailure(result) : NoContent();
        });
}

public sealed record UpdateCategoryRequest(
    [property: JsonRequired] string Name,
    [property: JsonRequired] int DisplayOrder);
