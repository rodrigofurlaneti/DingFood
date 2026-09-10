using FluentValidation;
namespace DingFood.Application.Features.Dining.Table.Deactivate
{
    public sealed class DeactivateDiningAreaTableCommandValidator : AbstractValidator<DeactivateDiningAreaTableCommand>
    {
        public DeactivateDiningAreaTableCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
