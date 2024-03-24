using Common.Domain;
using MediatR;

namespace Todos.Application.Queries.GetTodoById
{
    public class GetTodoByIdQuery: IRequest<ToDo?>
    {
        public int Id { get; set; }
    }
}
