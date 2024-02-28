using Common.Repositories;
using Todos.Domain;
using Todos.Repositories;

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

        public ToDo Create(ToDo todo)
        {
            var user = _userRepository.GetIdUser(todo.OwnerId);
            if (user == null)
                throw new Exception($"There isn't user with id {todo.OwnerId} in list");
            todo.CreatedDate = DateTime.UtcNow;
            return _todoRepository.AddTodo(todo);
        }
        public ToDo? Update(ToDo todo)
        {
            var user = _userRepository.GetIdUser(todo.OwnerId);
            if (user == null)
                throw new Exception($"There isn't user with id {todo.OwnerId} in list");
            var updtTo = _todoRepository.GetIdTodo(todo.Id);
            if (updtTo == null)
            {
                return null;
            }
            return _todoRepository.UpdateTodo(todo);
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
