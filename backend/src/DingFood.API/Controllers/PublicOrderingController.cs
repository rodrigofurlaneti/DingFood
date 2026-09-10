using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DingFood.Application.Features.Orders.AddItem;
using DingFood.Application.Features.PublicOrdering.AddItem;
using DingFood.Application.Features.PublicOrdering.GetPublicBill;
using DingFood.Application.Features.PublicOrdering.GetPublicComandaBill; // 1. Adicionado o using da nova feature
using DingFood.Application.Features.PublicOrdering.GetPublicMenu;
using DingFood.Application.Features.PublicOrdering.ValidateComandaReading;
using DingFood.Application.Features.PublicOrdering.ValidateTableReading;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace DingFood.API.Controllers;

[AllowAnonymous]
[EnableRateLimiting("public-ordering")]
public sealed class PublicOrderingController(
    IMediator mediator,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork,
    DingFood.Application.Abstractions.Security.IReadingProofService readingProof) : ApiController(mediator)
{
    [HttpGet("{token:guid}/menu")]
    public Task<IActionResult> GetMenu(Guid token, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PublicOrderingController), nameof(GetMenu), async () =>
        {
            var result = await Mediator.Send(new GetPublicMenuQuery(token), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpGet("{token:guid}/bill")]
    public Task<IActionResult> GetBill(Guid token, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PublicOrderingController), nameof(GetBill), async () =>
        {
            var result = await Mediator.Send(new GetPublicBillQuery(token), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpGet("{token:guid}/comandas/{code}/bill")]
    public Task<IActionResult> GetComandaBill(Guid token, string code, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PublicOrderingController), nameof(GetComandaBill), async () =>
        {
            var result = await Mediator.Send(new GetPublicComandaBillQuery(token, code), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
        });

    [HttpPost("{token:guid}/items")]
    public Task<IActionResult> AddItem(Guid token, [FromBody] AddPublicOrderItemRequest request, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PublicOrderingController), nameof(AddItem), async () =>
        {
            var result = await Mediator.Send(new AddPublicOrderItemCommand(
                token, request.ProductId, request.Quantity, request.Notes, request.Complements, request.ComandaCode, request.ReadingProof, request.ExpectedOrderId, request.OptionalExtraIds, request.BoostIds), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(new { orderId = result.Value });
        });


    [HttpPost("{token:guid}/comandas/{code}/reading-validation")]
    public Task<IActionResult> ValidateComandaReading(Guid token, string code, [FromBody] ValidateComandaReadingRequest request, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PublicOrderingController), nameof(ValidateComandaReading), async () =>
        {
            var result = await Mediator.Send(new ValidateComandaReadingCommand(
                token, code, request.Method, request.ScannedValue, request.PhotoBase64), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(new { proof = readingProof.Issue(token, code, request.Method) });
        });

    [HttpPost("{token:guid}/reading-validation")]
    public Task<IActionResult> ValidateTableReading(Guid token, [FromBody] ValidateComandaReadingRequest request, CancellationToken ct) =>
        ExecuteWithLogAsync(logRepository, unitOfWork, nameof(PublicOrderingController), nameof(ValidateTableReading), async () =>
        {
            var result = await Mediator.Send(new ValidateTableReadingCommand(
                token, request.Method, request.ScannedValue, request.PhotoBase64), ct);
            return result.IsFailure ? HandleFailure(result) : Ok(new { proof = readingProof.Issue(token, null, request.Method) });
        });
}

public sealed record AddPublicOrderItemRequest(
    [property: JsonRequired] long ProductId,
    [property: JsonRequired] decimal Quantity,
    string? Notes,
    IReadOnlyCollection<OrderItemComplementSelection>? Complements = null,
    string? ComandaCode = null,
    string? ReadingProof = null,
    long? ExpectedOrderId = null,
    IReadOnlyCollection<long>? OptionalExtraIds = null,
    IReadOnlyCollection<long>? BoostIds = null);

public sealed record ValidateComandaReadingRequest(
    [property: JsonRequired] string Method,
    string? ScannedValue,
    string? PhotoBase64);
