using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

public sealed class UploadIfoodImageCommandValidator : AbstractValidator<UploadIfoodImageCommand>
{
    public UploadIfoodImageCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.JsonBody).NotEmpty();
    }
}
