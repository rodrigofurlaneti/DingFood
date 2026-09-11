using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DingFood.API.Controllers;

[ApiController, Route("api/delivery"), Authorize]
public sealed class DeliveryController(AppDbContext db, ICurrentTenantService tenant) : ControllerBase
{
    private long BranchId => tenant.CompanyId.HasValue && tenant.BranchId.HasValue ? tenant.BranchId.Value : throw new DingFood.Domain.Exceptions.TenantAccessException();
    public sealed record DriverInput(string Name, string Phone, string VehiclePlate, string EmploymentType, bool IsActive = true);
    public sealed record ConditionInput(int DaysOfWeek, int StartMinute, int EndMinute, decimal PricePerKm, int Priority);
    public sealed record ConfigInput(string Model, decimal DailyAmount, decimal MaxRadiusKm, decimal PricePerKm, string TimeZoneId, List<ConditionInput> Conditions);
    public sealed record AssignmentInput(long DriverId);
    public sealed record DailyInput(long DriverId, DateOnly WorkDate);

    [HttpGet("drivers")]
    public async Task<IActionResult> Drivers(CancellationToken ct) => Ok(await db.Set<DeliveryDriver>().Where(x => x.BranchId == BranchId).OrderBy(x => x.Name).ToListAsync(ct));

    [HttpPost("drivers"), Authorize(Roles = ApiController.ManagerRoles)]
    public async Task<IActionResult> CreateDriver(DriverInput input, CancellationToken ct)
    {
        try {
            var driver = DeliveryDriver.Create(BranchId, input.Name, input.Phone, input.VehiclePlate, input.EmploymentType);
            db.Add(driver); await db.SaveChangesAsync(ct); return Ok(driver);
        } catch (ArgumentException e) { return BadRequest(new ProblemDetails { Detail = e.Message }); }
    }
    [HttpPut("drivers/{id:long}"), Authorize(Roles = ApiController.ManagerRoles)]
    public async Task<IActionResult> UpdateDriver(long id, DriverInput input, CancellationToken ct)
    {
        var driver = await db.Set<DeliveryDriver>().SingleOrDefaultAsync(x => x.Id == id && x.BranchId == BranchId, ct);
        if (driver is null) return NotFound();
        try { driver.Update(input.Name, input.Phone, input.VehiclePlate, input.EmploymentType, input.IsActive); await db.SaveChangesAsync(ct); return Ok(driver); }
        catch (ArgumentException e) { return BadRequest(new ProblemDetails { Detail = e.Message }); }
    }
    [HttpGet("config")]
    public async Task<IActionResult> Config(CancellationToken ct) => new JsonResult(await db.Set<DeliveryFeeConfig>().Include(x => x.Conditions).SingleOrDefaultAsync(x => x.BranchId == BranchId, ct));

    [HttpPut("config"), Authorize(Roles = ApiController.ManagerRoles)]
    public async Task<IActionResult> SaveConfig(ConfigInput input, CancellationToken ct)
    {
        try {
            if (input.Conditions is null || input.Conditions.Count > 100) return BadRequest();
            var conditions = input.Conditions.Select(x => DeliveryFeeCondition.Create(x.DaysOfWeek, x.StartMinute, x.EndMinute, x.PricePerKm, x.Priority)).ToList();
            if (conditions.GroupBy(x => x.Priority).Any(x => x.Count() > 1)) return BadRequest(new ProblemDetails { Detail = "Use prioridades diferentes para cada regra." });
            var config = await db.Set<DeliveryFeeConfig>().Include(x => x.Conditions).SingleOrDefaultAsync(x => x.BranchId == BranchId, ct);
            var isNew = config is null; config ??= DeliveryFeeConfig.Create(BranchId);
            config.Configure(input.Model, input.DailyAmount, input.MaxRadiusKm, input.PricePerKm, input.TimeZoneId);
            config.Conditions.Clear(); config.Conditions.AddRange(conditions);
            if (isNew) db.Add(config);
            await db.SaveChangesAsync(ct); return Ok(config);
        } catch (ArgumentException e) { return BadRequest(new ProblemDetails { Detail = e.Message }); }
    }

    [HttpGet("orders")]
    public async Task<IActionResult> Orders(CancellationToken ct) => Ok(await db.CustomerOrders.Where(x => x.BranchId == BranchId && x.IsActive && x.DeliveryFeeCalculatedAt != null)
        .OrderByDescending(x => x.Id).Take(200).Select(x => new { x.Id, x.CustomerName, x.DeliveryDriverId, x.DeliveryFeeAmount, x.DeliveryDistanceKm, x.DeliveryPaymentModel, x.OpenedAt, x.ClosedAt }).ToListAsync(ct));

    [HttpPut("orders/{id:long}/driver")]
    public async Task<IActionResult> Assign(long id, AssignmentInput input, CancellationToken ct)
    {
        var order = await db.CustomerOrders.SingleOrDefaultAsync(x => x.Id == id && x.BranchId == BranchId && x.IsActive, ct);
        var driver = await db.Set<DeliveryDriver>().SingleOrDefaultAsync(x => x.Id == input.DriverId && x.BranchId == BranchId && x.IsActive, ct);
        if (order is null || driver is null) return NotFound();
        try {
            order.AssignDeliveryDriver(driver);
            if (order.DeliveryPaymentModel == "Daily") {
                var date = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(order.DeliveryFeeCalculatedAt!.Value, DateTimeKind.Utc), TimeZoneInfo.FindSystemTimeZoneById(order.DeliveryTimeZoneId!)));
                if (!await db.Set<DeliveryDriverDailyPayment>().AnyAsync(x => x.BranchId == BranchId && x.DeliveryDriverId == driver.Id && x.WorkDate == date, ct))
                    db.Add(DeliveryDriverDailyPayment.Create(BranchId, driver.Id, date, order.DeliveryDailyAmount!.Value));
            }
            await db.SaveChangesAsync(ct); return NoContent();
        } catch (ArgumentException e) { return BadRequest(new ProblemDetails { Detail = e.Message }); }
        catch (DbUpdateException) { return Conflict(new ProblemDetails { Detail = "A atribuição foi alterada simultaneamente. Atualize e tente novamente." }); }
    }
    [HttpGet("daily-payments"), Authorize(Roles = ApiController.ManagerRoles)]
    public async Task<IActionResult> Payments(CancellationToken ct) => Ok(await db.Set<DeliveryDriverDailyPayment>().Where(x => x.BranchId == BranchId).OrderByDescending(x => x.WorkDate).Take(200).ToListAsync(ct));

    [HttpPost("daily-payments"), Authorize(Roles = ApiController.ManagerRoles)]
    public async Task<IActionResult> RegisterDaily(DailyInput input, CancellationToken ct)
    {
        var config = await db.Set<DeliveryFeeConfig>().SingleOrDefaultAsync(x => x.BranchId == BranchId, ct);
        if (config?.Model != "Daily") return BadRequest(new ProblemDetails { Detail = "A filial deve usar o modelo diária." });
        if (!await db.Set<DeliveryDriver>().AnyAsync(x => x.Id == input.DriverId && x.BranchId == BranchId && x.IsActive, ct)) return NotFound();
        var existing = await db.Set<DeliveryDriverDailyPayment>().SingleOrDefaultAsync(x => x.BranchId == BranchId && x.DeliveryDriverId == input.DriverId && x.WorkDate == input.WorkDate, ct);
        if (existing != null) return Ok(existing);
        var payment = DeliveryDriverDailyPayment.Create(BranchId, input.DriverId, input.WorkDate, config.DailyAmount);
        db.Add(payment);
        try { await db.SaveChangesAsync(ct); return Ok(payment); }
        catch (DbUpdateException) { return Conflict(new ProblemDetails { Detail = "Diária já registrada. Atualize a lista." }); }
    }
}
