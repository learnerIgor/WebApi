using Common.Domain;
using Todos.Service.Dto;

namespace Todos.Service
{
    public interface ITodoService
    {
        Task<ToDo> CreateAsync(int currentUserId, CreateToDoDto createTodo, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int currentUserId, int id, CancellationToken cancellationToken);
        Task<ToDo?> GetToDoByIdOrDefaultAsync(int currentUserId, int id, CancellationToken cancellationToken = default);
        Task<object> GetIsDoneTodoAsync(int currentUserId, int id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<ToDo>> GetListTodosAsync(int currentUserId, int? offset, string? labelFree, int? ownerTodo, int? limit = 7, CancellationToken cancellationToken = default);
        Task<ToDo> UpdateAsync(int currentUserId, int id, UpdateToDoDto updateTodo, CancellationToken cancellationToken);
        Task<object> PatchAsync(int currentUserId, int id, bool isDone, CancellationToken cancellationToken);
        Task<int> CountAsync(int currentUserId, int? userId, string? labelFree, CancellationToken cancellationToken);
    }
}