using Common.Domain;
using Todos.Service.Dto;

namespace Todos.Service
{
    public interface ITodoService
    {
        Task<ToDo> CreateAsync(CreateToDoDto createTodo, CancellationToken cancellationToken);
        void Delete(int id);
        Task<ToDo?> GetToDoByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<object> GetIsDoneTodoAsync(int id, CancellationToken cancellationToken);
        IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFree, int? ownerTodo, int? limit = 7);
        ToDo Update(UpdateToDoDto updateTodo);
        object Patch(int id, bool isDone);
        int Count(string? labelFree);
    }
}