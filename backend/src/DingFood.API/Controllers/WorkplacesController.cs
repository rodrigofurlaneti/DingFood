using System.Security.Claims;
using DingFood.Application.Abstractions.Tenancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DingFood.API.Controllers;

[ApiController, Route("api/workplaces"), Authorize(Policy = "AppUser")]
public sealed class WorkplacesController(IWorkplaceAccessService service) : ControllerBase
{
    private long UserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private long CompanyId => long.Parse(User.FindFirstValue("companyId")!);
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) => Ok(await service.GetAllowedAsync(UserId, CompanyId, ct));
    [HttpGet("brands"), Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Brands(CancellationToken ct) => Ok(await service.GetBrandsAsync(UserId, CompanyId, ct));
    [HttpPost, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create(CreateWorkplace request, CancellationToken ct)
    {
        var result = await service.CreateAsync(UserId, CompanyId, request, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error.Code, Detail = result.Error.Message });
    }
    [HttpPut("access"), Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Access(UpdateWorkplaceAccess request, CancellationToken ct)
    {
        var result = await service.SetAccessAsync(UserId, CompanyId, request, ct);
        return result.IsSuccess ? NoContent() : BadRequest(new ProblemDetails { Title = result.Error.Code, Detail = result.Error.Message });
    }
}
