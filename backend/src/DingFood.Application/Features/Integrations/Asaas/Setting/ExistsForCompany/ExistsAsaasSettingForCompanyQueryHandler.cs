using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingFood.Application.Features.Integrations.Asaas.Setting.ExistsForCompany
{
    internal sealed class ExistsAsaasSettingForCompanyQueryHandler
        : BaseQueryHandler<ExistsAsaasSettingForCompanyQuery, bool>
    {
        private readonly IAsaasIntegrationSettingRepository _settingRepository;

        public ExistsAsaasSettingForCompanyQueryHandler(
            IAsaasIntegrationSettingRepository settingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
        }

        public override async Task<Result<bool>> Handle(
            ExistsAsaasSettingForCompanyQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(ExistsAsaasSettingForCompanyQueryHandler),
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
