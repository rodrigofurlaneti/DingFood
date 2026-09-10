using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.ExistsForBranch
{
    internal sealed class ExistsBranchPaymentMethodSettingForBranchQueryHandler
        : BaseQueryHandler<ExistsBranchPaymentMethodSettingForBranchQuery, bool>
    {
        private readonly IBranchPaymentMethodSettingRepository _settingRepository;

        public ExistsBranchPaymentMethodSettingForBranchQueryHandler(
            IBranchPaymentMethodSettingRepository settingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
        }

        public override async Task<Result<bool>> Handle(
            ExistsBranchPaymentMethodSettingForBranchQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(ExistsBranchPaymentMethodSettingForBranchQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var exists = await _settingRepository.ExistsForBranchAsync(
                        request.BranchId,
                        cancellationToken);

                    return Result.Success(exists);
                });
        }
    }
}