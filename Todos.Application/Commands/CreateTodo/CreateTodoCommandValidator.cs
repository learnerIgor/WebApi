using FluentValidation;

namespace Todos.Application.Commands.CreateTodo
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator() 
        {
            RuleFor(l => l.Label).MinimumLength(5).MaximumLength(100).NotEmpty();
        }
    }
}
