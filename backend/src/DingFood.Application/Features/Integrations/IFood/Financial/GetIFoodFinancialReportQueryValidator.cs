using FluentValidation;
using DingFood.Application.Abstractions.Integrations.Ifood;

namespace DingFood.Application.Features.Integrations.Ifood.Financial;

public sealed class GetIfoodFinancialReportQueryValidator : AbstractValidator<GetIfoodFinancialReportQuery>
{
    public GetIfoodFinancialReportQueryValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.ReportType).IsInEnum();
    }
}
