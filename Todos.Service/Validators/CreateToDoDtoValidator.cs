using FluentValidation;
using Todos.Service.Dto;

namespace Todos.Service.Validators
{
    public class CreateToDoDtoValidator: AbstractValidator<CreateToDoDto>
    {
        public CreateToDoDtoValidator() 
        {
            RuleFor(l => l.Label).MinimumLength(5).MaximumLength(100).NotEmpty();
        }
    }
}
