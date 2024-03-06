using FluentValidation;
using Todos.Service.Dto;

namespace Todos.Service.Validators
{
    public class CreateToDoDtoValidator: AbstractValidator<CreateToDoDto>
    {
        public CreateToDoDtoValidator() 
        {
            RuleFor(o => o.OwnerId).GreaterThan(0).WithMessage("Incorrect OwnerId");
            RuleFor(l => l.Label).MinimumLength(10).MaximumLength(100).Must(l => l.StartsWith("Label"));
        }
    }
}
