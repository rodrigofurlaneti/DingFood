using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.OptionGroups;

public sealed class SetIfoodOptionStatusCommandValidator : AbstractValidator<SetIfoodOptionStatusCommand>
{
    public SetIfoodOptionStatusCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.OptionId).NotEmpty();
    }
}
