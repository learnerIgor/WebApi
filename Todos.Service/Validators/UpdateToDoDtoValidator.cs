using FluentValidation;
using Todos.Service.Dto;

namespace Todos.Service.Validators
{
    public class UpdateToDoDtoValidator: AbstractValidator<UpdateToDoDto>
    {
        public UpdateToDoDtoValidator() 
        {
            RuleFor(o => o.OwnerId).GreaterThan(0).WithMessage("Incorrect OwnerId");
            RuleFor(l => l.Label).MinimumLength(10).MaximumLength(100);
        }
    }
}
