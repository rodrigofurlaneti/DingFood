using FluentValidation;
namespace DingFood.Application.Features.Dining.Assignment.Create
{
    public sealed class CreateDiningAreaAssignmentCommandValidator : AbstractValidator<CreateDiningAreaAssignmentCommand>
    {
        public CreateDiningAreaAssignmentCommandValidator()
        {
            RuleFor(x => x.DiningAreaId).GreaterThan(0);
            RuleFor(x => x.EmployeeId).GreaterThan(0);
            RuleFor(x => x.StartAt).NotEmpty();
        }
    }
}
