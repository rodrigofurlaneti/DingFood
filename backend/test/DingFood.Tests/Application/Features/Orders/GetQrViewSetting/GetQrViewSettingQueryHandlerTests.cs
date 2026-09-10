using FluentAssertions;
using NSubstitute;
using DingFood.Application.Features.Orders;
using DingFood.Application.Features.Orders.GetQrViewSetting;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Xunit;

namespace DingFood.Tests.Application.Features.Orders.GetQrViewSetting;

public sealed class GetQrViewSettingQueryHandlerTests
{
    private readonly IDiningTableRepository _diningTableRepository = Substitute.For<IDiningTableRepository>();
    private readonly GetQrViewSettingQueryHandler _handler;

    public GetQrViewSettingQueryHandlerTests()
    {
        _handler = new GetQrViewSettingQueryHandler(_diningTableRepository);
    }

    [Fact]
    public async Task Handle_NoTablesInBranch_ReturnsEnabledTrueByDefault()
    {
        var query = new GetQrViewSettingQuery(BranchId: 1);
        _diningTableRepository.GetByBranchAsync(query.BranchId, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyCollection<DiningTable>)Array.Empty<DiningTable>());

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Enabled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_TablesExist_FirstTableQrViewEnabled_ReturnsTrue()
    {
        var table = DiningTable.Create(1, 1, 1, null).Value;
        var query = new GetQrViewSettingQuery(BranchId: 1);
        _diningTableRepository.GetByBranchAsync(query.BranchId, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyCollection<DiningTable>)new List<DiningTable> { table });

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Enabled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_TablesExist_FirstTableQrViewDisabled_ReturnsFalseUsingFirstTableOnly()
    {
        var firstTable = DiningTable.Create(1, 1, 1, null).Value;
        firstTable.SetQrViewEnabled(false);
        var secondTable = DiningTable.Create(1, 1, 2, null).Value;
        secondTable.SetQrViewEnabled(true);
        var query = new GetQrViewSettingQuery(BranchId: 1);
        _diningTableRepository.GetByBranchAsync(query.BranchId, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyCollection<DiningTable>)new List<DiningTable> { firstTable, secondTable });

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Enabled.Should().BeFalse();
    }
}
