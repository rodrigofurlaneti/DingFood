using DingFood.Domain.Constants;
using FluentValidation;

namespace DingFood.Application.Features.Cep.ConsultCep;

public sealed class ConsultCepCommandValidator : AbstractValidator<ConsultCepCommand>
{
    public ConsultCepCommandValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("Informe o CEP.")
            .Must(CepValidator.IsValid).WithMessage("O CEP informado é inválido.");
    }
}
