using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Commands.CreateUser;
using Users.Application.Commands.UpdateUser;
using Users.Application.Commands.UpdatePassword;
using Users.Application.Dto;
using Users.Application.Queries.GetCounts;
using Users.Application.Queries.GetListUsers;
using Users.Application.Queries.GetUserById;
using Users.Application.Commands.DeleteUser;
using MediatR;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ListUsers(
            [FromQuery] GetListUsersQuery getListUsersQuery, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {
            var users = await mediator.Send(getListUsersQuery, cancellationToken);
            var count = await mediator.Send(new GetCountUsersQuery() { NameFree = getListUsersQuery.NameFree }, cancellationToken);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("Id")]
        public async Task<IActionResult> GetUserById(
            [FromQuery] GetUserByIdQuery getUserByIdQuery, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {
            var user = await mediator.Send(getUserByIdQuery, cancellationToken);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("UsersCount")]
        public async Task<IActionResult> GetCount(
            [FromQuery] BaseUsersFilter baseUsersFilter, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetCountUsersQuery() { NameFree = baseUsersFilter.NameFree }, cancellationToken));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserCommand createUserCommand, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {
            var userNew = await mediator.Send(createUserCommand, cancellationToken);
            return Created($"/User/{userNew.Id}", userNew);
        }

        [HttpPut("Id")]
        public async Task<IActionResult> UpdateUser(
            [FromQuery] int id,
            [FromBody] UpdateUserCommandPayLoad updateUserCommandPayLoad, 
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            UpdateUserCommand updateUserCommand = new(id, updateUserCommandPayLoad);
            var userUpdt = await mediator.Send(updateUserCommand, cancellationToken);
            return Ok(userUpdt);
        }

        [HttpPut("Id/Password")]
        public async Task<IActionResult> UpdateUserPassword(
            [FromQuery] int id,
            [FromBody] UpdatePasswordPayLoad updatePasswordPayLoad, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {
            UpdatePasswordCommand updatePasswordCommand = new(id, updatePasswordPayLoad);
            var userUpdt = await mediator.Send(updatePasswordCommand, cancellationToken);
            return Ok(userUpdt);
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteUser(
            [FromQuery] DeleteUserCommand deleteUserCommand,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(deleteUserCommand, cancellationToken);
            return Ok(result);
        }
    }
}