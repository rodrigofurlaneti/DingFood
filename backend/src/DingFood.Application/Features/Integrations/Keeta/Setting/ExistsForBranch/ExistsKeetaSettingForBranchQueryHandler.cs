using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.ExistsForBranch
{
    internal sealed class ExistsKeetaSettingForBranchQueryHandler
        : BaseQueryHandler<ExistsKeetaSettingForBranchQuery, bool>
    {
        private readonly IKeetaIntegrationSettingRepository _settingRepository;

        public ExistsKeetaSettingForBranchQueryHandler(
            IKeetaIntegrationSettingRepository settingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
        }

        public override async Task<Result<bool>> Handle(
            ExistsKeetaSettingForBranchQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(ExistsKeetaSettingForBranchQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var exists = await _settingRepository.ExistsForBranchAsync(request.BranchId, cancellationToken);
                    return Result.Success(exists);
                });
        }
    }
}
