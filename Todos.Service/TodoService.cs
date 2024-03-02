using Common.Repositories;
using Todos.Domain;
using Todos.Repositories;
using Todos.Service.Dto;

namespace Todos.Service
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IUserRepository _userRepository;

        public TodoService(ITodoRepository todoRepository, IUserRepository userRepository)
        {
            _todoRepository = todoRepository;
            _userRepository = userRepository;
        }

        public IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7)
        {
            return _todoRepository.GetListTodos(offset, labelFreeText, ownerTodo, limit);
        }

        public ToDo? GetIdTodo(int id)
        {
            return _todoRepository.GetIdTodo(id);
        }

        public ToDo Create(CreateToDoDto createTodo)
        {
            var user = _userRepository.GetIdUser(createTodo.OwnerId);
            if (user == null)
                throw new Exception($"There isn't user with id {createTodo.OwnerId} in list");

            var _todoEntity = new ToDo()
            {
                Label = createTodo.Label,
                IsDone = createTodo.IsDone,
                OwnerId = createTodo.OwnerId,
                CreatedDate = DateTime.UtcNow
            };

            return _todoRepository.AddTodo(_todoEntity);
        }
        public ToDo? Update(UpdateToDoDto updateTodo)
        {
            var user = _userRepository.GetIdUser(updateTodo.OwnerId);
            if (user == null)
                throw new Exception($"There isn't user with id {updateTodo.OwnerId} in list");
            var _todoEntity = _todoRepository.GetIdTodo(updateTodo.Id);
            if (_todoEntity == null)
            {
                return null;
            }
            _todoEntity.Label = updateTodo.Label;
            _todoEntity.IsDone = updateTodo.IsDone;
            _todoEntity.OwnerId = updateTodo.OwnerId;
            _todoEntity.UpdatedDate = DateTime.UtcNow;

            return _todoRepository.UpdateTodo(_todoEntity);
        }
        public bool Delete(int id)
        {
            var deletTo = this.GetIdTodo(id);
            if (deletTo == null)
                return false;

            return _todoRepository.DeleteTodoById(deletTo);
        }
    }
}
