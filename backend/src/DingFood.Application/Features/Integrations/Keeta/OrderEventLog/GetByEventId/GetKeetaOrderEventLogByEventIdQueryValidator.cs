using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetByEventId
{
    public sealed class GetKeetaOrderEventLogByEventIdQueryValidator
        : AbstractValidator<GetKeetaOrderEventLogByEventIdQuery>
    {
        public GetKeetaOrderEventLogByEventIdQueryValidator()
        {
            RuleFor(x => x.EventId).NotEmpty()
                .WithMessage("O EventId é obrigatório.");
        }
    }
}
