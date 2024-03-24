using MediatR;

namespace Todos.Application.Queries.GetTodoIsDone
{
    public class GetTodoIsDoneQuery: IRequest<object>
    {
        public int Id { get; set; }
    }
}
