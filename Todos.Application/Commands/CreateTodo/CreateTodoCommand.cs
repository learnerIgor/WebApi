using Common.Domain;
using MediatR;

namespace Todos.Application.Commands.CreateTodo
{
    public class CreateTodoCommand: IRequest<ToDo>
    {
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
    }
}
