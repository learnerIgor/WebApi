using FluentValidation;

namespace Todos.Application.Commands.DeleteTodo
{
    public class DeleteTodoCommandValidator: AbstractValidator<DeleteTodoCommand>
    {
        public DeleteTodoCommandValidator() 
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
        }
    }
}
