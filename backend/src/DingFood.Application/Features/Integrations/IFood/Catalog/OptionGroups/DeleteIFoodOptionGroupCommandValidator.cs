using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.OptionGroups;

public sealed class DeleteIfoodOptionGroupCommandValidator : AbstractValidator<DeleteIfoodOptionGroupCommand>
{
    public DeleteIfoodOptionGroupCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.OptionGroupId).NotEmpty();
    }
}
