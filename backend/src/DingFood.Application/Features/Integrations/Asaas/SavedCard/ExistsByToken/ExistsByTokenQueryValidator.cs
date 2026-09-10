using FluentValidation;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.ExistsByToken
{
    public sealed class ExistsByTokenQueryValidator : AbstractValidator<ExistsByTokenQuery>
    {
        public ExistsByTokenQueryValidator()
        {
            RuleFor(x => x.CreditCardToken)
                .NotEmpty()
                .WithMessage("O CreditCardToken é obrigatório.")
                .MaximumLength(150)
                .WithMessage("O CreditCardToken deve ter no máximo 150 caracteres.");
        }
    }
}
