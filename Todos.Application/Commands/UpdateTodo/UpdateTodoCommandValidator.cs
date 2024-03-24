using FluentValidation;

namespace Todos.Application.Commands.UpdateTodo
{
    public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
    {
        public UpdateTodoCommandValidator()
        {
            RuleFor(l => l.Label).MinimumLength(5).MaximumLength(100).NotEmpty();
        }
    }
}
