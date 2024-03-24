using MediatR;

namespace Todos.Application.Commands.DeleteTodo
{
    public class DeleteTodoCommand: IRequest<bool>
    {
        public int Id { get; set; }
    }
}
