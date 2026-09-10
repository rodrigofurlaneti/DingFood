using FluentAssertions;
using Moq;
using Reqnroll;
using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Reconhecer alerta operacional do Ifood")]
public sealed class AcknowledgeIFoodOperationalAlertCommandSteps
{
    private readonly Mock<IIfoodOperationalAlertStore> _alertStore = new();
    private readonly Mock<ILogTrackerRepository> _logRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private readonly Guid _alertId = Guid.NewGuid();
    private Result? _result;

    [Given(@"existe um alerta operacional pendente para a empresa (.*)")]
    public void GivenExisteUmAlertaOperacionalPendenteParaAEmpresa(long companyId)
        => _alertStore.Setup(s => s.Acknowledge(companyId, _alertId)).Returns(true);

    [Given(@"nao existe mais o alerta operacional para a empresa (.*)")]
    public void GivenNaoExisteMaisOAlertaOperacionalParaAEmpresa(long companyId)
        => _alertStore.Setup(s => s.Acknowledge(companyId, _alertId)).Returns(false);

    [When(@"eu tento reconhecer o alerta operacional da empresa (.*)")]
    public async Task WhenEuTentoReconhecerOAlertaOperacionalDaEmpresa(long companyId)
    {
        var handler = new AcknowledgeIfoodOperationalAlertCommandHandler(
            _alertStore.Object, _logRepository.Object, _unitOfWork.Object);

        _result = await handler.Handle(new AcknowledgeIfoodOperationalAlertCommand(companyId, _alertId), CancellationToken.None);
    }

    [Then(@"a operacao deve ter sucesso")]
    public void ThenAOperacaoDeveTerSucesso()
        => _result!.IsSuccess.Should().BeTrue();
}
