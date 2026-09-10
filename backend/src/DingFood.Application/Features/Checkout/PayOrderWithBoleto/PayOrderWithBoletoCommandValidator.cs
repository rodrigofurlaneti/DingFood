using FluentValidation;

namespace DingFood.Application.Features.Checkout.PayOrderWithBoleto
{
    public sealed class PayOrderWithBoletoCommandValidator : AbstractValidator<PayOrderWithBoletoCommand>
    {
        public PayOrderWithBoletoCommandValidator()
        {
            RuleFor(x => x.CustomerOrderId)
                .GreaterThan(0)
                .WithMessage("O identificador do pedido (CustomerOrderId) deve ser maior que zero.");
        }
    }
}
