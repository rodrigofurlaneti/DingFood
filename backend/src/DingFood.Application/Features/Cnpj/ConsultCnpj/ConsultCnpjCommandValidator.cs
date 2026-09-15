using DingFood.Domain.Constants;
using FluentValidation;

namespace DingFood.Application.Features.Cnpj.ConsultCnpj;

public sealed class ConsultCnpjCommandValidator : AbstractValidator<ConsultCnpjCommand>
{
    public ConsultCnpjCommandValidator()
    {
        RuleFor(x => x.TaxId)
            .NotEmpty().WithMessage("Informe o CNPJ.")
            .Must(CnpjValidator.IsValid).WithMessage("O CNPJ informado é inválido.");
    }
}
