using FluentAssertions;
using NSubstitute;
using DingFood.Application.Features.Integrations.Asaas.SavedCard.GetById;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Asaas.SavedCard.GetById;

public sealed class GetAsaasSavedCardByIdQueryHandlerTests
{
    private readonly IAsaasIntegrationSavedCardRepository _savedCardRepository = Substitute.For<IAsaasIntegrationSavedCardRepository>();
    private readonly ILogTrackerRepository _logRepository = Substitute.For<ILogTrackerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly GetAsaasSavedCardByIdQueryHandler _handler;

    public GetAsaasSavedCardByIdQueryHandlerTests()
    {
        _handler = new GetAsaasSavedCardByIdQueryHandler(_savedCardRepository, _logRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_CardNotFound_ShouldReturnNotFound()
    {
        _savedCardRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns((AsaasIntegrationSavedCard?)null);

        var result = await _handler.Handle(new GetAsaasSavedCardByIdQuery(1), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("AsaasSavedCard.NotFound");
    }

    [Fact]
    public async Task Handle_CardFound_ShouldReturnMappedResponse()
    {
        var card = AsaasIntegrationSavedCard.Create(1, 1, "token", "VISA", "1111").Value;
        _savedCardRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(card);

        var result = await _handler.Handle(new GetAsaasSavedCardByIdQuery(1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.CardBrand.Should().Be("VISA");
        result.Value.Last4Digits.Should().Be("1111");
    }
}
