using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Todos.Service;
using Todos.Service.Dto;

namespace Todos.Api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetListTodos(int? offset, string? LabelFreeText, int? ownerTodo, int? limit, CancellationToken cancellationToken)
        {
            var todos = await _todoService.GetListTodosAsync(offset, LabelFreeText, ownerTodo, limit, cancellationToken);
            var count = await _todoService.CountAsync(ownerTodo, LabelFreeText, cancellationToken);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id, CancellationToken cancellationToken)
        {
            var todo = await _todoService.GetToDoByIdOrDefaultAsync(id, cancellationToken);
            return Ok(todo);
        }

        [HttpGet("{id}/IsDone")]
        public async Task<IActionResult> GetIsDone(int id, CancellationToken cancellationToken)
        {
            var isDone = await _todoService.GetIsDoneTodoAsync(id, cancellationToken);
            return Ok(isDone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(CreateToDoDto createTodoDto, CancellationToken cancellationToken)
        {
            var newToDo = await _todoService.CreateAsync(createTodoDto, cancellationToken);
            return Created($"ToDo/{newToDo.Id}", newToDo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, UpdateToDoDto updtTodoDto, CancellationToken cancellationToken)
        {
            var updateTodo = await _todoService.UpdateAsync(id, updtTodoDto, cancellationToken);
            return Ok(updateTodo);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodo([FromBody] int id, CancellationToken cancellationToken)
        {
            var result = await _todoService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{id}/isDone")]
        public async Task<IActionResult> PatchIsDone(int id, [FromBody] bool UpdateDone, CancellationToken cancellationToken)
        {
            var isDone = await _todoService.PatchAsync(id, UpdateDone, cancellationToken);
            return Ok(isDone);
        }
    }
}
