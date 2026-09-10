using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetByAuthId
{
    public sealed class GetKeetaAuthorizationSessionByAuthIdQueryValidator
        : AbstractValidator<GetKeetaAuthorizationSessionByAuthIdQuery>
    {
        public GetKeetaAuthorizationSessionByAuthIdQueryValidator()
        {
            RuleFor(x => x.AuthId).NotEmpty()
                .WithMessage("O AuthId é obrigatório.");
        }
    }
}
