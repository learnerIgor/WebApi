using Todos.Domain;

namespace Todos.Repositories
{
    public interface ITodoRepository
    {
        ToDo? GetIdTodo(int id);
        IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFree, int? ownerTodo, int? limit);
        ToDo AddTodo(ToDo toDo);
        ToDo UpdateTodo(ToDo toDo);
        bool DeleteTodoById(ToDo toDo);
    }
}