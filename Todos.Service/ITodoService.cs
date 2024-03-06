using Todos.Domain;
using Todos.Service.Dto;

namespace Todos.Service
{
    public interface ITodoService
    {
        ToDo Create(CreateToDoDto createTodo);
        bool Delete(int id);
        ToDo? GetIdTodo(int id);
        object? GetIsDoneTodo(int id);
        IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFree, int? ownerTodo, int? limit = 7);
        ToDo? Update(UpdateToDoDto updateTodo);
        object? Patch(int id, bool isDone);
        int Count(string? labelFree);
    }
}