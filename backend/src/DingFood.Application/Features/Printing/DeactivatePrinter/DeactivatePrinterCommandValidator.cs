using FluentValidation;

namespace DingFood.Application.Features.Printing.DeactivatePrinter;

public sealed class DeactivatePrinterCommandValidator : AbstractValidator<DeactivatePrinterCommand>
{
    public DeactivatePrinterCommandValidator()
    {
        RuleFor(x => x.PrinterId).GreaterThan(0);
    }
}
