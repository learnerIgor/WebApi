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
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public TodoService(IRepository<ToDo> todoRepository, IRepository<User> userRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ToDo>> GetListTodosAsync(int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7, CancellationToken cancellationToken = default)
        {
            return await _todoRepository.GetListAsync(
                offset, 
                limit, t => (string.IsNullOrWhiteSpace(labelFreeText) || t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase)) && (ownerTodo == null || t.UserId == ownerTodo), 
                t => t.Id, 
                cancellationToken: cancellationToken);
        }

        public async Task<ToDo?> GetToDoByIdOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        {
            ToDo? todo = await _todoRepository.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (todo == null)
            {
                Log.Error($"There isn't todo with id {id} in list");
                throw new NotFoundException(new { Id = id });
            }
            return todo;
        }

        public async Task<object> GetIsDoneTodoAsync(int id, CancellationToken cancellationToken)
        {
            ToDo? todoDone = await _todoRepository.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (todoDone == null)
            {
                Log.Error($"There isn't todo with id {id} in list");
                throw new NotFoundException($"There isn't todo with id {id} in list");
            }

            return new { Id = todoDone.Id, IsDone = todoDone.IsDone };
        }

        public async Task<ToDo> CreateAsync(CreateToDoDto createTodo, CancellationToken cancellationToken)
        {
            var user = await _userRepository.SingleOrDefaultAsync(i => i.Id == createTodo.UserId, cancellationToken);
            if (user == null)
            {
                Log.Error($"There isn't user with id {createTodo.UserId} in list");
                throw new BadRequestException($"There isn't user with id {createTodo.UserId} in list");
            }
            var todoEntity = _mapper.Map<CreateToDoDto, ToDo>(createTodo);
            todoEntity.CreatedDate = DateTime.UtcNow;
            Log.Information("Added new todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.AddAsync(todoEntity, cancellationToken);
        }

        public async Task<ToDo> UpdateAsync(UpdateToDoDto updateTodo, CancellationToken cancellationToken)
        {
            var user = await _userRepository.SingleOrDefaultAsync(i => i.Id == updateTodo.UserId, cancellationToken);
            if (user == null)
            {
                Log.Error($"There isn't user with id {updateTodo.UserId} in list");
                throw new BadRequestException($"There isn't user with id {updateTodo.UserId} in list");
            }
            var todoEntity = await GetToDoByIdOrDefaultAsync(updateTodo.Id, cancellationToken);
            _mapper.Map(updateTodo, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            Log.Information("Updated todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
        }

        public async Task<object> PatchAsync(int id, bool isDone, CancellationToken cancellationToken)
        {
            ToDo? todo = await _todoRepository.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (todo == null)
            {
                Log.Error($"There todo with id {id} in list");
                throw new BadRequestException($"There isn't todo with id {id} in list");
            }
            todo.IsDone = isDone;
            await _todoRepository.UpdateAsync(todo, cancellationToken);
            Log.Information("Todo updated using Patch method " + JsonConvert.SerializeObject(todo));

            return new { Id = todo.Id, IsDone = todo.IsDone };
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var deletTo = await GetToDoByIdOrDefaultAsync(id, cancellationToken);
            Log.Information("Deleted todo " + JsonConvert.SerializeObject(deletTo));
            return await _todoRepository.DeleteAsync(deletTo, cancellationToken);
        }

        public async Task<int> CountAsync(string? labelFree, CancellationToken cancellationToken)
        {
            return await _todoRepository.CountAsync(labelFree == null ? null : c => c.Label.Contains(labelFree), cancellationToken);
        }
    }
}
