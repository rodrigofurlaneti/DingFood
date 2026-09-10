using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetPendingDisputesByBranch
{
    public sealed class GetPendingKeetaRefundDisputesByBranchQueryValidator
        : AbstractValidator<GetPendingKeetaRefundDisputesByBranchQuery>
    {
        public GetPendingKeetaRefundDisputesByBranchQueryValidator()
        {
            RuleFor(x => x.BranchId).GreaterThan(0)
                .WithMessage("O identificador da filial (BranchId) deve ser maior que zero.");
        }
    }
}
