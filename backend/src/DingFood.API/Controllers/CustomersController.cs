using System.Security.Claims;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DingFood.Application.Features.Customers.AddLoyaltyPoints;
using DingFood.Application.Features.Customers.Create;
using DingFood.Application.Features.Customers.GetByCompany;
using DingFood.Domain.Repositories;

namespace DingFood.API.Controllers;

public sealed class CustomersController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    [HttpGet("company/{companyId:long}")]
    public Task<IActionResult> GetByCompany(long companyId, [FromQuery] string? search, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CustomersController), nameof(GetByCompany), async () =>
        {
            var result = await Mediator.Send(new GetCustomersByCompanyQuery(companyId, search), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CustomersController), nameof(Create), async () =>
        {
            var result = await Mediator.Send(command, ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpPut("{id:long}/loyalty-points")]
    public Task<IActionResult> AddLoyaltyPoints(long id, [FromBody] AddLoyaltyPointsRequest request, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CustomersController), nameof(AddLoyaltyPoints), async () =>
        {
            var result = await Mediator.Send(new AddLoyaltyPointsCommand(id, request.Points), ct);
            return result.IsFailure ? HandleFailure(result) : NoContent();
        });
}

public sealed record AddLoyaltyPointsRequest([property: JsonRequired] int Points);