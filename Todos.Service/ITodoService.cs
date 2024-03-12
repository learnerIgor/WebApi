using Common.Domain;
using Todos.Service.Dto;

namespace Todos.Service
{
    public interface ITodoService
    {
        Task<ToDo> CreateAsync(CreateToDoDto createTodo, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<ToDo?> GetToDoByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<object> GetIsDoneTodoAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<ToDo>> GetListTodosAsync(int? offset, string? labelFree, int? ownerTodo, int? limit = 7, CancellationToken cancellationToken = default);
        Task<ToDo> UpdateAsync(UpdateToDoDto updateTodo, CancellationToken cancellationToken);
        Task<object> PatchAsync(int id, bool isDone, CancellationToken cancellationToken);
        int Count(string? labelFree);
    }
}