using Microsoft.AspNetCore.Mvc;
using Todos.Domain;
using Todos.Service;
using Todos.Service.Dto;

namespace Todos.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("todos")]
        public IActionResult GetListTodos(int? offset, string? LabelFreeText, int? ownerTodo, int? limit)
        {
            var todos = _todoService.GetListTodos(offset, LabelFreeText, ownerTodo, limit);
            return Ok(todos);
        }
        [HttpGet("todos/{id}")]
        public IActionResult GetIdTodo(int id)
        {
            var todo = _todoService.GetIdTodo(id);
            if (todo != null)
                return Ok(todo);
            else
                return NotFound($"{id}");
        }

        [HttpPost("todos")]
        public IActionResult CreateTodo(CreateToDoDto createTodoDto)
        {
            var newToDo = _todoService.Create(createTodoDto);
            return Created($"ToDo/todos/{newToDo.Id}", createTodoDto);
        }
        [HttpPut("todos/{id}")]
        public IActionResult UpdateTodo(int id, UpdateToDoDto updtTodoDto)
        {
            updtTodoDto.Id = id;
            var updateTodo = _todoService.Update(updtTodoDto);
            if (updateTodo == null)
                return NotFound($"{id}");
            return Ok(updateTodo);
        }
        [HttpDelete("todos")]
        public IActionResult DeleteTodo([FromBody] int id)
        {
            var deleteTodo = _todoService.Delete(id);
            return deleteTodo ? Ok() : NotFound();
        }
    }
}
