using FluentValidation;

namespace Todos.Application.Commands.UpdateIsDone
{
    public class UpdateIsDoneCommandValidator:AbstractValidator<UpdateIsDoneCommand>
    {
        public UpdateIsDoneCommandValidator() 
        {
            RuleFor(i => i.IsDone).Must(x => x == false || x == true);
        }
    }
}
