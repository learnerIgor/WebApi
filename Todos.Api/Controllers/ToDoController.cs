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

        [HttpGet]
        public IActionResult GetListTodos(int? offset, string? LabelFreeText, int? ownerTodo, int? limit)
        {
            var todos = _todoService.GetListTodos(offset, LabelFreeText, ownerTodo, limit);
            var count = _todoService.Count(LabelFreeText);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetIdTodo(int id)
        {
            var todo = _todoService.GetIdTodo(id);
            return Ok(todo);
        }

        [HttpGet("UsersCount")]
        public IActionResult GetCount(string? labelFree)
        {
            return Ok(_todoService.Count(labelFree));
        }

        [HttpGet("{id}/IsDone")]
        public IActionResult GetIsDone(int id)
        {
            var isDone = _todoService.GetIsDoneTodo(id);
            return Ok(isDone);
        }

        [HttpPost]
        public IActionResult CreateTodo(CreateToDoDto createTodoDto)
        {
            var newToDo = _todoService.Create(createTodoDto);
            return Created($"ToDo/{newToDo.Id}", newToDo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, UpdateToDoDto updtTodoDto)
        {
            updtTodoDto.Id = id;
            var updateTodo = _todoService.Update(updtTodoDto);
            return Ok(updateTodo);
        }

        [HttpDelete]
        public IActionResult DeleteTodo([FromBody] int id)
        {
            _todoService.Delete(id);
            return Ok();
        }

        [HttpPatch("{id}/isDone")]
        public IActionResult PatchIsDone(int id, [FromBody] bool UpdateDone)
        {
            var isDone = _todoService.Patch(id, UpdateDone);
            return Ok(isDone);
        }
    }
}
