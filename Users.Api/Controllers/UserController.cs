using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Service;
using Users.Service.Dto;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ListUsers(int? offset, string? nameFree, int? limit, CancellationToken cancellationToken)
        {
            var users = await _userService.GetListUsersAsync(offset, nameFree, limit, cancellationToken);
            var count = await _userService.CountAsync(nameFree, cancellationToken);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task <IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdOrDefaultAsync(id, cancellationToken);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("UsersCount")]
        public async Task<IActionResult> GetCount(string? nameFree, CancellationToken cancellationToken)
        {
            return Ok(await _userService.CountAsync(nameFree, cancellationToken));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto user, CancellationToken cancellationToken)
        {
            var userNew = await _userService.CreateAsync(user, cancellationToken);
            return Created($"/User/{userNew.Id}", userNew);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user, int id, CancellationToken cancellationToken)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var userUpdt = await _userService.UpdateAsync(currentUserId, id, user, cancellationToken);
            return Ok(userUpdt);
        }

        [HttpPut("{id}/Password")]
        public async Task<IActionResult> UpdateUserPassword(UpdatePasswordDto user, int id, CancellationToken cancellationToken)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var userUpdt = await _userService.UpdatePasswordAsync(currentUserId, id, user, cancellationToken);
            return Ok(userUpdt);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.DeleteAsync(currentUserId, id, cancellationToken);
            return Ok(result);
        }
    }
}