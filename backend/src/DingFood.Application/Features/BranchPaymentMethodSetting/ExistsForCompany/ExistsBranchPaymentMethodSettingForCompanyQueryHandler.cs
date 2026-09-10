using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.ExistsForCompany
{
    internal sealed class ExistsBranchPaymentMethodSettingForCompanyQueryHandler
        : BaseQueryHandler<ExistsBranchPaymentMethodSettingForCompanyQuery, bool>
    {
        private readonly IBranchPaymentMethodSettingRepository _settingRepository;

        public ExistsBranchPaymentMethodSettingForCompanyQueryHandler(
            IBranchPaymentMethodSettingRepository settingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
        }

        public override async Task<Result<bool>> Handle(
            ExistsBranchPaymentMethodSettingForCompanyQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(ExistsBranchPaymentMethodSettingForCompanyQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var exists = await _settingRepository.ExistsForCompanyAsync(
                        request.CompanyId,
                        cancellationToken);

                    return Result.Success(exists);
                });
        }
    }
}