using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActiveByCompanyId
{
    internal sealed class GetAllActiveByCompanyIdBranchPaymentMethodSettingQueryHandler
        : BaseQueryHandler<GetAllActiveByCompanyIdBranchPaymentMethodSettingQuery, IReadOnlyList<BranchPaymentMethodSettingResponse>>
    {
        private readonly IBranchPaymentMethodSettingRepository _settingRepository;

        public GetAllActiveByCompanyIdBranchPaymentMethodSettingQueryHandler(
            IBranchPaymentMethodSettingRepository settingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
        }

        public override async Task<Result<IReadOnlyList<BranchPaymentMethodSettingResponse>>> Handle(
            GetAllActiveByCompanyIdBranchPaymentMethodSettingQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetAllActiveByCompanyIdBranchPaymentMethodSettingQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var settings = await _settingRepository.GetAllActiveByCompanyIdAsync(
                        request.CompanyId,
                        cancellationToken);

                    var response = settings.Select(s => new BranchPaymentMethodSettingResponse(
                        s.Id,
                        s.CompanyId,
                        s.BranchId,
                        s.EnablePix,
                        s.EnableBoleto,
                        s.EnableCreditCard,
                        s.EnableDebitCard,
                        s.EnableCashMachine,
                        s.IsActive, s.CreatedAt, s.UpdatedAt)).ToList();

                    return Result.Success<IReadOnlyList<BranchPaymentMethodSettingResponse>>(response);
                });
        }
    }
}
