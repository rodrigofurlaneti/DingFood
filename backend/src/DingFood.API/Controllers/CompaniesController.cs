using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DingFood.Application.Features.Companies.Register;
using DingFood.Domain.Repositories;

namespace DingFood.API.Controllers;

[Authorize(Policy = "AppUser")]
public sealed class CompaniesController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
{
    [Authorize]
    [HttpGet("allowed")]
    public async Task<IActionResult> GetAllowed(CancellationToken ct)
    {
        var result = await Mediator.Send(new DingFood.Application.Features.Companies.GetAllowedCompaniesQuery(), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("group")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CreateInGroup([FromBody] DingFood.Application.Features.Companies.CreateGroupCompanyCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [Authorize]
    [HttpPost("switch")]
    public async Task<IActionResult> Switch([FromBody] DingFood.Application.Features.Companies.SwitchCompanyCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("group/users")]
    public async Task<IActionResult> GroupUsers(CancellationToken ct)
    {
        var result = await Mediator.Send(new DingFood.Application.Features.Companies.GetGroupUsersQuery(), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("access")]
    public async Task<IActionResult> SetAccess([FromBody] DingFood.Application.Features.Companies.SetCompanyAccessCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    [HttpPost("register")]
    public Task<IActionResult> Register([FromBody] RegisterCompanyCommand command, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(CompaniesController), nameof(Register), async () =>
        {
            var result = await Mediator.Send(command, ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });
}
