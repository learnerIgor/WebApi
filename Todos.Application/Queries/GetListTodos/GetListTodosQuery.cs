using Common.Domain;
using MediatR;

namespace Todos.Application.Queries.GetListTodos
{
    public class GetListTodosQuery: IRequest<IReadOnlyCollection<ToDo>>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public int? OwnerTodo { get; set; }
        public string? LabelFreeText { get; set; }
    }
}
