using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Security;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood;

internal sealed class TestIfoodConnectionCommandHandler : BaseCommandHandler<TestIfoodConnectionCommand, TestIfoodConnectionResponse>
{
    private const string ProtectorPurpose = "DingFood.Integrations.Ifood.ClientSecret.v1";

    private readonly IIfoodIntegrationSettingRepository _settingRepository;
    private readonly IIfoodAuthClient _authClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretProtector _secretProtector;

    public TestIfoodConnectionCommandHandler(
        IIfoodIntegrationSettingRepository settingRepository,
        IIfoodAuthClient authClient,
        ISecretProtector secretProtector,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _settingRepository = settingRepository;
        _authClient = authClient;
        _unitOfWork = unitOfWork;
        _secretProtector = secretProtector;
    }

    public override async Task<Result<TestIfoodConnectionResponse>> Handle(
        TestIfoodConnectionCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(TestIfoodConnectionCommandHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                var setting = await _settingRepository.GetByCompanyForUpdateAsync(request.CompanyId, cancellationToken);
                if (setting?.ClientId is null || setting.ClientSecretEncrypted is null)
                    return Result.Success(new TestIfoodConnectionResponse(
                        false, "Cadastre o Client ID e o Client Secret antes de testar a conexão."));

                string clientSecret;
                try
                {
                    clientSecret = _secretProtector.Unprotect(ProtectorPurpose, setting.ClientSecretEncrypted);
                }
                catch (Exception)
                {
                    // Chave de proteção mudou/foi perdida (ex.: reset de ambiente) — não crasha,
                    // orienta a recadastrar.
                    return Result.Success(new TestIfoodConnectionResponse(
                        false, "Não foi possível ler o segredo salvo — cadastre o Client Secret novamente."));
                }

                var auth = await _authClient.AuthenticateAsync(setting.ClientId, clientSecret, cancellationToken);

                setting.RegisterConnectionTest(auth.Success);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(new TestIfoodConnectionResponse(auth.Success, auth.ErrorMessage));
            });
    }
}
