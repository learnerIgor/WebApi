using FluentValidation;
using Todos.Service.Dto;

namespace Todos.Service.Validators
{
    public class UpdateToDoDtoValidator: AbstractValidator<UpdateToDoDto>
    {
        public UpdateToDoDtoValidator() 
        {
            RuleFor(l => l.Label).MinimumLength(5).MaximumLength(100).NotEmpty();
        }
    }
}
