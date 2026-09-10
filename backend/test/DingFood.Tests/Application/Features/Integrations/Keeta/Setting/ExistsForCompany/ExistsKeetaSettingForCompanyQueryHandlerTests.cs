using FluentAssertions;
using NSubstitute;
using DingFood.Application.Features.Integrations.Keeta.Setting.ExistsForCompany;
using DingFood.Domain.Repositories;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.Setting.ExistsForCompany;

public sealed class ExistsKeetaSettingForCompanyQueryHandlerTests
{
    private readonly IKeetaIntegrationSettingRepository _settingRepository = Substitute.For<IKeetaIntegrationSettingRepository>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly ExistsKeetaSettingForCompanyQueryHandler _handler;

    public ExistsKeetaSettingForCompanyQueryHandlerTests()
    {
        _handler = new ExistsKeetaSettingForCompanyQueryHandler(_settingRepository, _logRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_SettingExists_ShouldReturnTrue()
    {
        _settingRepository.ExistsForCompanyAsync(1, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(new ExistsKeetaSettingForCompanyQuery(1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SettingDoesNotExist_ShouldReturnFalse()
    {
        _settingRepository.ExistsForCompanyAsync(1, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _handler.Handle(new ExistsKeetaSettingForCompanyQuery(1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeFalse();
    }
}
