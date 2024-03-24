using MediatR;

namespace Todos.Application.Queries.GetCountTodos
{
    public class GetCountTodosQuery: IRequest<int>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public int? OwnerTodo { get; set; }
        public string? LabelFreeText { get; set; }
    }
}
