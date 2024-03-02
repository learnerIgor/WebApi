﻿using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Todos.Domain;
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
            todoRepository.Add(new ToDo { Id = 1, Label = "Label1", IsDone = false, OwnerId = 1 });
            todoRepository.Add(new ToDo { Id = 2, Label = "Label2", IsDone = true, OwnerId = 2 });
            todoRepository.Add(new ToDo { Id = 3, Label = "Label3", IsDone = false, OwnerId = 3 });
            todoRepository.Add(new ToDo { Id = 4, Label = "Label4", IsDone = true, OwnerId = 4 });
            todoRepository.Add(new ToDo { Id = 5, Label = "Label5", IsDone = false, OwnerId = 5 });
            todoRepository.Add(new ToDo { Id = 6, Label = "Label6", IsDone = true, OwnerId = 6 });
            todoRepository.Add(new ToDo { Id = 7, Label = "Label7", IsDone = false, OwnerId = 7 });
            todoRepository.Add(new ToDo { Id = 8, Label = "Label8", IsDone = true, OwnerId = 8 });
            todoRepository.Add(new ToDo { Id = 9, Label = "Label9", IsDone = false, OwnerId = 9 });
            todoRepository.Add(new ToDo { Id = 10, Label = "Label10", IsDone = true, OwnerId = 10 });
            todoRepository.Add(new ToDo { Id = 11, Label = "Label10", IsDone = true, OwnerId = 3 });
            todoRepository.Add(new ToDo { Id = 12, Label = "Label7", IsDone = true, OwnerId = 3 });


            userRepository.Add(new User { Id = 1, Name = "Tom" });
            userRepository.Add(new User { Id = 2, Name = "Bob" });
            userRepository.Add(new User { Id = 3, Name = "Allice" });
            userRepository.Add(new User { Id = 4, Name = "John" });
            userRepository.Add(new User { Id = 5, Name = "Marty" });
            userRepository.Add(new User { Id = 6, Name = "Lionel" });
            userRepository.Add(new User { Id = 7, Name = "Garry" });
            userRepository.Add(new User { Id = 8, Name = "Tim" });
            userRepository.Add(new User { Id = 9, Name = "Max" });
            userRepository.Add(new User { Id = 10, Name = "Berta" });
        }

        public IReadOnlyCollection<ToDo> GetListTodos(int? offset, string? labelFreeText, int? ownerTodo, int? limit = 7)
        {
            return _todoRepository.GetList(
                offset,
                limit,
                t => (string.IsNullOrWhiteSpace(labelFreeText) || t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase)) && (ownerTodo == null || t.OwnerId == ownerTodo),
                t=>t.Id);
        }

        public ToDo? GetIdTodo(int id)
        {
            return _todoRepository.SingleOrDefault(i => i.Id == id);
        }

        public ToDo Create(CreateToDoDto createTodo)
        {
            var user = _userRepository.SingleOrDefault(i => i.Id == createTodo.OwnerId);
            if (user == null)
                throw new Exception($"There isn't user with id {createTodo.OwnerId} in list");

            var todoEntity = _mapper.Map<CreateToDoDto, ToDo>(createTodo);
            todoEntity.CreatedDate = DateTime.UtcNow;
            todoEntity.Id = _todoRepository.GetList().Count() == 0 ? 1 : _todoRepository.GetList().Max(i => i.Id) + 1;

            return _todoRepository.Add(todoEntity);
        }
        public ToDo? Update(UpdateToDoDto updateTodo)
        {
            var user = _userRepository.SingleOrDefault(i => i.Id == updateTodo.OwnerId);
            if (user == null)
                throw new Exception($"There isn't user with id {updateTodo.OwnerId} in list");
            var todoEntity = GetIdTodo(updateTodo.Id);
            if (todoEntity == null)
            {
                return null;
            }

            _mapper.Map(updateTodo, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;

            return _todoRepository.Update(todoEntity);
        }
        public bool Delete(int id)
        {
            var deletTo = GetIdTodo(id);
            if (deletTo == null)
                return false;

            return _todoRepository.Delete(deletTo);
        }

        public int Count(string? labelFree)
        {
            return _todoRepository.Count(labelFree == null ? null : c => c.Label.Contains(labelFree, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
