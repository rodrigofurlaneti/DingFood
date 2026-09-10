using FluentAssertions;
using NSubstitute;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetAllByCompanyId;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.MerchantMapping.GetAllByCompanyId;

public sealed class GetAllKeetaMerchantMappingsByCompanyIdQueryHandlerTests
{
    private readonly IKeetaIntegrationMerchantMappingRepository _mappingRepository = Substitute.For<IKeetaIntegrationMerchantMappingRepository>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly GetAllKeetaMerchantMappingsByCompanyIdQueryHandler _handler;

    public GetAllKeetaMerchantMappingsByCompanyIdQueryHandlerTests()
    {
        _handler = new GetAllKeetaMerchantMappingsByCompanyIdQueryHandler(_mappingRepository, _logRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_MappingsExist_ShouldReturnMappedList()
    {
        var mappings = new List<KeetaIntegrationMerchantMapping>
        {
            KeetaIntegrationMerchantMapping.Create(1, 2, "im-1", 100, "Loja 1").Value,
            KeetaIntegrationMerchantMapping.Create(1, 3, "im-2", 200, "Loja 2").Value,
        };
        _mappingRepository.GetAllByCompanyIdAsync(1, Arg.Any<CancellationToken>()).Returns(mappings);

        var result = await _handler.Handle(new GetAllKeetaMerchantMappingsByCompanyIdQuery(1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_NoMappingsForCompany_ShouldReturnEmptyList()
    {
        _mappingRepository.GetAllByCompanyIdAsync(1, Arg.Any<CancellationToken>()).Returns([]);

        var result = await _handler.Handle(new GetAllKeetaMerchantMappingsByCompanyIdQuery(1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
