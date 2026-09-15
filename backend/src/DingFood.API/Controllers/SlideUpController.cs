using DingFood.Application.Features.Companies.Register;
using DingFood.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DingFood.API.Controllers
{
    public sealed class SlideUpController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork) : ApiController(mediator)
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public Task<IActionResult> Register([FromBody] RegisterCompanyCommand command, CancellationToken ct) =>
            ExecuteWithLogAsync(logRepository, unitOfWork, nameof(SlideUpController), nameof(Register), async () =>
            {
                var result = await Mediator.Send(command, ct);
                return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
            });
    }
}


