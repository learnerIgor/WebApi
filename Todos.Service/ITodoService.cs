using Todos.Domain;

namespace Todos.Service
{
    public interface ITodoService
    {
        ToDo Create(ToDo todo);
        bool Delete(int id);
        ToDo? GetIdTodo(int id);
        IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFree, int? ownerTodo, int? limit = 7);
        ToDo? Update(ToDo todo);
    }
}