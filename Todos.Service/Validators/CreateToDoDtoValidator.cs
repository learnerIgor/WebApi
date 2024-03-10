using FluentValidation;
using Todos.Service.Dto;

namespace Todos.Service.Validators
{
    public class CreateToDoDtoValidator: AbstractValidator<CreateToDoDto>
    {
        public CreateToDoDtoValidator() 
        {
            RuleFor(o => o.UserId).GreaterThan(0).WithMessage("Incorrect UserId");
            RuleFor(l => l.Label).MinimumLength(10).MaximumLength(100);
        }
    }
}
