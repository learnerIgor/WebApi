using Todos.Domain;

namespace Todos.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private static List<ToDo> TodoList = new()
        {
                new ToDo { Id = 1, Label = "Label1", IsDone = false, OwnerId=1 },
                new ToDo { Id = 2, Label = "Label2", IsDone = true, OwnerId=2 },
                new ToDo { Id = 3, Label = "Label3", IsDone = false, OwnerId = 3 },
                new ToDo { Id = 4, Label = "Label4", IsDone = true, OwnerId = 4 },
                new ToDo { Id = 5, Label = "Label5", IsDone = false, OwnerId = 5 },
                new ToDo { Id = 6, Label = "Label6", IsDone = true, OwnerId = 6 },
                new ToDo { Id = 7, Label = "Label7", IsDone = false, OwnerId = 7 },
                new ToDo { Id = 8, Label = "Label8", IsDone = true, OwnerId = 8 },
                new ToDo { Id = 9, Label = "Label9", IsDone = false, OwnerId = 9 },
                new ToDo { Id = 10, Label = "Label10", IsDone = true, OwnerId = 10 }
        };

        public IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7)
        {
            var todos = TodoList;

            ownerTodo = ownerTodo ?? 0;

            if (!string.IsNullOrWhiteSpace(labelFreeText))
            {
                todos = todos.Where(l => l.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase) && l.OwnerId == ownerTodo).ToList();
            }

            todos = todos.OrderBy(i => i.Id).ToList();

            if (offset != null)
                todos = todos.Skip(offset.Value).ToList();

            limit ??= 7;

            todos = todos.Take(limit.Value).ToList();
            return todos;
        }

        public ToDo? GetIdTodo(int id)
        {
            return TodoList.Where(i => i.Id == id).SingleOrDefault();
        }

        public ToDo AddTodo(ToDo toDo)
        {
            toDo.Id = TodoList.Max(i => i.Id) + 1;
            TodoList.Add(toDo);
            return toDo;
        }

        public ToDo UpdateTodo(ToDo toDo)
        {
            var updateTodo = TodoList.Single(i => i.Id == toDo.Id);
            updateTodo.Label = toDo.Label;
            return updateTodo;
        }

        public bool DeleteTodoById(ToDo toDo)
        {
            var deleteTodo = TodoList.SingleOrDefault(i => i.Id == toDo.Id);
            if (deleteTodo != null)
            {
                TodoList.Remove(deleteTodo);
                return true;
            }

            return false;
        }
    }
}
