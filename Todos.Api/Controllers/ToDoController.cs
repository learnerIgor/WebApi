﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Commands.CreateTodo;
using Todos.Application.Commands.DeleteTodo;
using Todos.Application.Commands.UpdateIsDone;
using Todos.Application.Commands.UpdateTodo;
using Todos.Application.Dtos;
using Todos.Application.Queries.GetCountTodos;
using Todos.Application.Queries.GetListTodos;
using Todos.Application.Queries.GetTodoById;
using Todos.Application.Queries.GetTodoIsDone;

namespace Todos.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetListTodos(
            [FromQuery] GetListTodosQuery getListTodosQuery,
            [FromQuery] GetCountTodosQuery getCountTodosQuery,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var todos = await mediator.Send(getListTodosQuery, cancellationToken);
            var count = await mediator.Send(getCountTodosQuery, cancellationToken);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(
            int id,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var todo = await mediator.Send(new GetTodoByIdQuery { Id = id }, cancellationToken);
            return Ok(todo);
        }

        [HttpGet("{id}/IsDone")]
        public async Task<IActionResult> GetIsDone(
            int id,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var isDone = await mediator.Send(new GetTodoIsDoneQuery { Id = id }, cancellationToken);
            return Ok(isDone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(
            [FromBody] CreateTodoCommand createTodoDto,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var newToDo = await mediator.Send(createTodoDto, cancellationToken);
            return Created($"ToDo/{newToDo.Id}", newToDo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(
            int id, 
            [FromBody] UpdateTodoPayLoad updateTodoPayLoad,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            UpdateTodoCommand updateTodoCommand = new(id, updateTodoPayLoad);
            var updateTodo = await mediator.Send(updateTodoCommand, cancellationToken);
            return Ok(updateTodo);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodo(
            [FromBody] DeleteTodoCommand deleteTodoCommand,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(deleteTodoCommand, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{id}/isDone")]
        public async Task<IActionResult> PatchIsDone(
            int id, 
            [FromBody] UpdateIsDonePayLoad updateIsDonePayLoad,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            UpdateIsDoneCommand updateIsDoneCommand = new(id, updateIsDonePayLoad);
            var isDone = await mediator.Send(updateIsDoneCommand, cancellationToken);
            return Ok(isDone);
        }
    }
}
