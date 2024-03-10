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

            if (_userRepository.GetList().Length == 0)
            {
                userRepository.Add(new User { Name = "Tom" });
                userRepository.Add(new User { Name = "Bob" });
                userRepository.Add(new User { Name = "Allice" });
                userRepository.Add(new User { Name = "John" });
                userRepository.Add(new User { Name = "Marty" });
                userRepository.Add(new User { Name = "Lionel" });
                userRepository.Add(new User { Name = "Garry" });
                userRepository.Add(new User { Name = "Tim" });
                userRepository.Add(new User { Name = "Max" });
                userRepository.Add(new User { Name = "Berta" });
            }

            if (_todoRepository.GetList().Length == 0)
            {
                todoRepository.Add(new ToDo { Label = "Label1", IsDone = false, UserId = 1 });
                todoRepository.Add(new ToDo { Label = "Label2", IsDone = true, UserId = 2 });
                todoRepository.Add(new ToDo { Label = "Label3", IsDone = false, UserId = 3 });
                todoRepository.Add(new ToDo { Label = "Label4", IsDone = true, UserId = 4 });
                todoRepository.Add(new ToDo { Label = "Label5", IsDone = false, UserId = 5 });
                todoRepository.Add(new ToDo { Label = "Label6", IsDone = true, UserId = 6 });
                todoRepository.Add(new ToDo { Label = "Label7", IsDone = false, UserId = 7 });
                todoRepository.Add(new ToDo { Label = "Label8", IsDone = true, UserId = 8 });
                todoRepository.Add(new ToDo { Label = "Label9", IsDone = false, UserId = 9 });
                todoRepository.Add(new ToDo { Label = "Label10", IsDone = true, UserId = 10 });
                todoRepository.Add(new ToDo { Label = "Label10", IsDone = true, UserId = 3 });
                todoRepository.Add(new ToDo { Label = "Label7", IsDone = true, UserId = 3 });
            }
        }

        public IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7)
        {
            return _todoRepository.GetList(
                offset,
                limit,
                t => (string.IsNullOrWhiteSpace(labelFreeText) || t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase)) && (ownerTodo == null || t.UserId == ownerTodo),
                t => t.Id);
        }

        public ToDo GetIdTodo(int id)
        {
            var todo = _todoRepository.SingleOrDefault(i => i.Id == id);
            if (todo == null)
            {
                Log.Error($"There isn't todo with id {id} in list");
                throw new NotFoundException(new { Id = id });
            }

            return todo;
        }

        public async Task<ToDo?> GetIdTodoAsync(int id, CancellationToken cancellationToken = default)
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

        public ToDo Update(UpdateToDoDto updateTodo)
        {
            var user = _userRepository.SingleOrDefault(i => i.Id == updateTodo.UserId);
            if (user == null)
            {
                Log.Error($"There isn't user with id {updateTodo.UserId} in list");
                throw new BadRequestException($"There isn't user with id {updateTodo.UserId} in list");
            }
            var todoEntity = GetIdTodo(updateTodo.Id);
            _mapper.Map(updateTodo, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            Log.Information("Updated todo " + JsonConvert.SerializeObject(todoEntity));

            return _todoRepository.Update(todoEntity);
        }

        public object Patch(int id, bool isDone)
        {
            ToDo? todo = _todoRepository.SingleOrDefault(x => x.Id == id);
            if (todo == null)
            {
                Log.Error($"There todo with id {id} in list");
                throw new BadRequestException($"There isn't todo with id {id} in list");
            }
            todo.IsDone = isDone;
            _todoRepository.Update(todo);
            Log.Information("Todo updated using Patch method " + JsonConvert.SerializeObject(todo));

            return new { Id = todo.Id, IsDone = todo.IsDone };
        }

        public void Delete(int id)
        {
            var deletTo = GetIdTodo(id);
            Log.Information("Deleted todo " + JsonConvert.SerializeObject(deletTo));
            _todoRepository.Delete(deletTo);
        }

        public int Count(string? labelFree)
        {
            return _todoRepository.Count(labelFree == null ? null : c => c.Label.Contains(labelFree));
        }
    }
}
