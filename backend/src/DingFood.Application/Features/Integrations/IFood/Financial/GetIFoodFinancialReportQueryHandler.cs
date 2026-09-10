using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Financial;

internal sealed class GetIfoodFinancialReportQueryHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodFinancialClient financialClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetIfoodFinancialReportQuery, IfoodFinancialReportResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodFinancialReportResponse>> Handle(
        GetIfoodFinancialReportQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetIfoodFinancialReportQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodFinancialReportResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;

                IfoodFinancialReportResultDto result;
                if (request.ReportType == IfoodFinancialReportType.AnticipationsV3)
                {
                    result = await financialClient.GetAnticipationsAsync(token, merchantId, cancellationToken);
                }
                else if (request.ReportType == IfoodFinancialReportType.SalesV3)
                {
                    var end = request.RangeEnd ?? DateTime.Today;
                    var start = request.RangeStart ?? end.AddDays(-30);
                    result = await financialClient.GetSalesV3Async(token, merchantId, start, end, page: 1, cancellationToken);
                }
                else
                {
                    result = await financialClient.GetReportAsync(
                        token, merchantId, request.ReportType, request.PeriodId, request.RangeStart, request.RangeEnd, cancellationToken);
                }

                return Result.Success(new IfoodFinancialReportResponse(
                    request.ReportType.ToString(), result.RawItems.Count, result.RawItems));
            });
    }
}
