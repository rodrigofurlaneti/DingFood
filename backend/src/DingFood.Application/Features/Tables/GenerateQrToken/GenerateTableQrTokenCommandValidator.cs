using FluentValidation;

namespace DingFood.Application.Features.Tables.GenerateQrToken;

public sealed class GenerateTableQrTokenCommandValidator : AbstractValidator<GenerateTableQrTokenCommand>
{
    public GenerateTableQrTokenCommandValidator()
    {
        RuleFor(x => x.DiningTableId).GreaterThan(0);
    }
}
