using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service;
using Common.Service.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Todos.Service.Dto;

namespace Todos.Service
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public TodoService(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ToDo>> GetListTodosAsync(int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7, CancellationToken cancellationToken = default)
        {
            var checkAdmin = _currentUserService.UserRole.Contains("Admin");

            return await _todoRepository.GetListAsync(
                offset,
                limit,
                t => (string.IsNullOrWhiteSpace(labelFreeText) || t.Label.Contains(labelFreeText))
                && (ownerTodo == null || t.UserId == ownerTodo)
                && (_currentUserService.CurrentUserId == t.UserId || checkAdmin),
                t => t.Id,
                cancellationToken: cancellationToken);
        }

        public async Task<ToDo?> GetToDoByIdOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        {
            ToDo? todo = await _todoRepository.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (todo == null)
            {
                Log.Error($"There isn't todo with id {id} in DB");
                throw new NotFoundException(new { Id = id });
            }
            if (_currentUserService.CurrentUserId != todo.UserId && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {id}");
                throw new ForbiddenException();
            }

            return todo;
        }

        public async Task<object> GetIsDoneTodoAsync(int id, CancellationToken cancellationToken)
        {
            ToDo? todoDone = await _todoRepository.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (todoDone == null)
            {
                Log.Error($"There isn't todo with id {id} in list");
                throw new NotFoundException($"There isn't todo with id {id} in DB");
            }
            if (_currentUserService.CurrentUserId != todoDone.UserId && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {id}");
                throw new ForbiddenException();
            }

            return new { Id = todoDone.Id, IsDone = todoDone.IsDone };
        }

        public async Task<ToDo> CreateAsync(CreateToDoDto createTodo, CancellationToken cancellationToken)
        {
            var todoEntity = _mapper.Map<CreateToDoDto, ToDo>(createTodo);
            todoEntity.CreatedDate = DateTime.UtcNow;
            todoEntity.UserId = _currentUserService.CurrentUserId;
            Log.Information("Added new todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.AddAsync(todoEntity, cancellationToken);
        }

        public async Task<ToDo> UpdateAsync(int id, UpdateToDoDto updateTodo, CancellationToken cancellationToken)
        {
            var todoEntity = await GetToDoByIdOrDefaultAsync(id, cancellationToken);
            _mapper.Map(updateTodo, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            Log.Information("Updated todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
        }

        public async Task<object> PatchAsync(int id, bool isDone, CancellationToken cancellationToken)
        {
            var todoEntity = await GetToDoByIdOrDefaultAsync(id, cancellationToken);
            todoEntity.IsDone = isDone;
            await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
            Log.Information("Todo updated using Patch method " + JsonConvert.SerializeObject(todoEntity));

            return new { Id = todoEntity.Id, IsDone = todoEntity.IsDone };
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var deletTo = await GetToDoByIdOrDefaultAsync(id, cancellationToken);
            Log.Information("Deleted todo " + JsonConvert.SerializeObject(deletTo));
            return await _todoRepository.DeleteAsync(deletTo, cancellationToken);
        }

        public async Task<int> CountAsync(int? userId, string? labelFree, CancellationToken cancellationToken)
        {
            var checkAdmin = _currentUserService.UserRole.Contains("Admin");
            return await _todoRepository.CountAsync(
                t => (string.IsNullOrWhiteSpace(labelFree) || t.Label.Contains(labelFree))
                && (userId == null || t.UserId == userId)
                && (_currentUserService.CurrentUserId == t.UserId || checkAdmin), cancellationToken);
        }
    }
}
