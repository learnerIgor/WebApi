using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Todos.Service.Dto;

namespace Todos.Service
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly IRepository<ApplicationUserApplicationRole> _appUserAppRoleRepository;
        private readonly IMapper _mapper;

        public TodoService(IRepository<ToDo> todoRepository, IRepository<ApplicationUserApplicationRole> appUserAppRoleRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _appUserAppRoleRepository = appUserAppRoleRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ToDo>> GetListTodosAsync(int currentUserId, int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7, CancellationToken cancellationToken = default)
        {
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: u => u.ApplicationUserId == currentUserId, cancellationToken: cancellationToken);
            var checkAdmin = userRoles.Any(t => t.ApplicationUserRole.Name == "Admin");

            return await _todoRepository.GetListAsync(
                offset,
                limit,
                t => (string.IsNullOrWhiteSpace(labelFreeText) || t.Label.Contains(labelFreeText))
                && (ownerTodo == null || t.UserId == ownerTodo)
                && (currentUserId == t.UserId || checkAdmin),
                t => t.Id,
                cancellationToken: cancellationToken);
        }

        public async Task<ToDo?> GetToDoByIdOrDefaultAsync(int currentUserId, int id, CancellationToken cancellationToken = default)
        {
            ToDo? todo = await _todoRepository.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (todo == null)
            {
                Log.Error($"There isn't todo with id {id} in DB");
                throw new NotFoundException(new { Id = id });
            }
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: x => x.ApplicationUserId == currentUserId, cancellationToken: cancellationToken);
            if (currentUserId != todo.UserId && !userRoles.Any(u => u.ApplicationUserRole.Name == "Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {id}");
                throw new ForbiddenException();
            }

            return todo;
        }

        public async Task<object> GetIsDoneTodoAsync(int currentUserId, int id, CancellationToken cancellationToken)
        {
            ToDo? todoDone = await _todoRepository.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (todoDone == null)
            {
                Log.Error($"There isn't todo with id {id} in list");
                throw new NotFoundException($"There isn't todo with id {id} in DB");
            }
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: x => x.ApplicationUserId == currentUserId, cancellationToken: cancellationToken);
            if (currentUserId != todoDone.UserId && !userRoles.Any(u => u.ApplicationUserRole.Name == "Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {id}");
                throw new ForbiddenException();
            }

            return new { Id = todoDone.Id, IsDone = todoDone.IsDone };
        }

        public async Task<ToDo> CreateAsync(int currentUserId, CreateToDoDto createTodo, CancellationToken cancellationToken)
        {
            var todoEntity = _mapper.Map<CreateToDoDto, ToDo>(createTodo);
            todoEntity.CreatedDate = DateTime.UtcNow;
            todoEntity.UserId = currentUserId;
            Log.Information("Added new todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.AddAsync(todoEntity, cancellationToken);
        }

        public async Task<ToDo> UpdateAsync(int currentUserId, int id, UpdateToDoDto updateTodo, CancellationToken cancellationToken)
        {
            var todoEntity = await GetToDoByIdOrDefaultAsync(currentUserId, id, cancellationToken);
            _mapper.Map(updateTodo, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            Log.Information("Updated todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
        }

        public async Task<object> PatchAsync(int currentUserId, int id, bool isDone, CancellationToken cancellationToken)
        {
            var todoEntity = await GetToDoByIdOrDefaultAsync(currentUserId, id, cancellationToken);
            todoEntity.IsDone = isDone;
            await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
            Log.Information("Todo updated using Patch method " + JsonConvert.SerializeObject(todoEntity));

            return new { Id = todoEntity.Id, IsDone = todoEntity.IsDone };
        }

        public async Task<bool> DeleteAsync(int currentUserId, int id, CancellationToken cancellationToken)
        {
            var deletTo = await GetToDoByIdOrDefaultAsync(currentUserId, id, cancellationToken);
            Log.Information("Deleted todo " + JsonConvert.SerializeObject(deletTo));
            return await _todoRepository.DeleteAsync(deletTo, cancellationToken);
        }

        public async Task<int> CountAsync(int currentUserId, int? userId, string? labelFree, CancellationToken cancellationToken)
        {
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: x => x.ApplicationUserId == currentUserId, cancellationToken: cancellationToken);
            var checkAdmin = userRoles.Any(t => t.ApplicationUserRole.Name == "Admin");
            return await _todoRepository.CountAsync(
                t => (string.IsNullOrWhiteSpace(labelFree) || t.Label.Contains(labelFree))
                && (userId == null || t.UserId == userId)
                && (currentUserId == t.UserId || checkAdmin), cancellationToken);
        }
    }
}
