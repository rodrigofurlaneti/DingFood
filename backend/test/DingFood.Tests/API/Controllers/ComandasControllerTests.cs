using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using DingFood.API.Controllers;
using DingFood.Application.Features.Comandas;
using DingFood.Application.Features.Comandas.GetByBranch;
using DingFood.Application.Features.Comandas.Settings;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
using DingFood.Tests.API.Controllers.TestSupport;
using Xunit;

namespace DingFood.Tests.API.Controllers;

public sealed class ComandasControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ComandasController _controller;

    public ComandasControllerTests()
    {
        _controller = new ComandasController(_mediator, _logRepository, _unitOfWork);
        ControllerTestHelpers.AttachHttpContext(_controller);
    }

    [Fact]
    public async Task GetByBranch_Success_ShouldSendQueryAndReturnOk()
    {
        _mediator.Send(Arg.Is<GetComandasByBranchQuery>(q => q.BranchId == 5), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyCollection<ComandaResponse>>([]));

        var result = await _controller.GetByBranch(5, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetByBranch_Failure_ShouldReturnMappedErrorResult()
    {
        _mediator.Send(Arg.Any<GetComandasByBranchQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<IReadOnlyCollection<ComandaResponse>>(new Error("Branch.NotFound", "filial nao encontrada")));

        var result = await _controller.GetByBranch(999, CancellationToken.None);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetSettings_Success_ShouldSendQueryAndReturnOk()
    {
        _mediator.Send(Arg.Is<GetComandaSettingQuery>(q => q.BranchId == 5), Arg.Any<CancellationToken>())
            .Returns(Result.Success(new ComandaSettingResponse(100m)));

        var result = await _controller.GetSettings(5, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetSettings_Failure_ShouldReturnMappedErrorResult()
    {
        _mediator.Send(Arg.Any<GetComandaSettingQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<ComandaSettingResponse>(new Error("Branch.NotFound", "filial nao encontrada")));

        var result = await _controller.GetSettings(999, CancellationToken.None);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task SetDefaultLimit_Success_ShouldReturnNoContent()
    {
        var command = new SetComandaDefaultLimitCommand(5, 150m);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(Result.Success());

        var result = await _controller.SetDefaultLimit(command, CancellationToken.None);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task SetDefaultLimit_Failure_ShouldReturnMappedErrorResult()
    {
        var command = new SetComandaDefaultLimitCommand(999, 150m);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(Result.Failure(new Error("Branch.NotFound", "filial nao encontrada")));

        var result = await _controller.SetDefaultLimit(command, CancellationToken.None);

        result.Should().BeOfType<NotFoundObjectResult>();
    }
}
