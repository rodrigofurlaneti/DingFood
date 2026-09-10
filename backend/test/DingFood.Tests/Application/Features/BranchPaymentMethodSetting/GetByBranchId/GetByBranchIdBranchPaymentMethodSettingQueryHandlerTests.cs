using FluentAssertions;
using NSubstitute;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetByBranchId;
using DingFood.Domain.Repositories;
using Xunit;
using SettingEntity = DingFood.Domain.Entities.BranchPaymentMethodSetting;

namespace DingFood.Tests.Application.Features.BranchPaymentMethodSetting.GetByBranchId;

public sealed class GetByBranchIdBranchPaymentMethodSettingQueryHandlerTests
{
    private readonly IBranchPaymentMethodSettingRepository _settingRepository = Substitute.For<IBranchPaymentMethodSettingRepository>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly GetByBranchIdBranchPaymentMethodSettingQueryHandler _handler;

    public GetByBranchIdBranchPaymentMethodSettingQueryHandlerTests()
    {
        _handler = new GetByBranchIdBranchPaymentMethodSettingQueryHandler(_settingRepository, _logRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_SettingExists_ShouldReturnMappedResponse()
    {
        var setting = SettingEntity.Create(companyId: 1, branchId: 2).Value;
        _settingRepository.GetByBranchIdAsync(2, Arg.Any<CancellationToken>()).Returns(setting);

        var result = await _handler.Handle(new GetByBranchIdBranchPaymentMethodSettingQuery(2), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.BranchId.Should().Be(2);
    }

    [Fact]
    public async Task Handle_SettingNotFound_ShouldReturnSuccessWithNull()
    {
        _settingRepository.GetByBranchIdAsync(2, Arg.Any<CancellationToken>()).Returns((SettingEntity?)null);

        var result = await _handler.Handle(new GetByBranchIdBranchPaymentMethodSettingQuery(2), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }
}
