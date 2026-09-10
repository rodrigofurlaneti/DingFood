using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetByOrderId
{
    public sealed class GetKeetaRefundDisputeByOrderIdQueryValidator
        : AbstractValidator<GetKeetaRefundDisputeByOrderIdQuery>
    {
        public GetKeetaRefundDisputeByOrderIdQueryValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty()
                .WithMessage("O OrderId é obrigatório.");
        }
    }
}
