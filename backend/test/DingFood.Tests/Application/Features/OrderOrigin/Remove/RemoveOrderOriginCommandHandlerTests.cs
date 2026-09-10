using FluentAssertions;
using NSubstitute;
using DingFood.Application.Features.OrderOrigin.Remove;
using DingFood.Domain.Repositories;
using Xunit;

namespace DingFood.Tests.Application.Features.OrderOrigin.Remove;

public sealed class RemoveOrderOriginCommandHandlerTests
{
    private readonly IOrderOriginRepository _orderOriginRepository = Substitute.For<IOrderOriginRepository>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly RemoveOrderOriginCommandHandler _handler;

    public RemoveOrderOriginCommandHandlerTests()
    {
        _handler = new RemoveOrderOriginCommandHandler(_orderOriginRepository, _logRepository, _unitOfWork);
    }

    private static DingFood.Domain.Entities.OrderOrigin CreateOrigin()
        => DingFood.Domain.Entities.OrderOrigin.Create(1, 1, "LOCAL", DateTime.Now).Value;

    [Fact]
    public async Task Handle_OrderOriginNotFound_ShouldReturnFailure()
    {
        _orderOriginRepository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns((DingFood.Domain.Entities.OrderOrigin?)null);

        var result = await _handler.Handle(new RemoveOrderOriginCommand(1), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("OrderOrigin.NotFound");
        _orderOriginRepository.DidNotReceive().Remove(Arg.Any<DingFood.Domain.Entities.OrderOrigin>());
        // A falha em si ainda passa pelo commit do log de auditoria do BaseCommandHandler.
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_OrderOriginFound_ShouldRemoveAndCommit()
    {
        var origin = CreateOrigin();
        _orderOriginRepository.GetByIdForUpdateAsync(1, Arg.Any<CancellationToken>()).Returns(origin);

        var result = await _handler.Handle(new RemoveOrderOriginCommand(1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _orderOriginRepository.Received(1).Remove(origin);
        // 1 commit do save de negócio + 1 commit do log de auditoria gravado pelo BaseCommandHandler.
        await _unitOfWork.Received(2).CommitAsync(Arg.Any<CancellationToken>());
    }
}
