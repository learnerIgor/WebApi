using Common.Domain;
using Microsoft.AspNetCore.Mvc;
using Users.Service;
using Users.Service.Dto;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> ListUsers(int? offset, string? nameFree, int? limit, CancellationToken cancellationToken)
        {
            var users = await _userService.GetListUsersAsync(offset, nameFree, limit, cancellationToken);
            var count = _userService.Count(nameFree);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [HttpGet("UsersCount")]
        public IActionResult GetCount(string? nameFree)
        {
            return Ok(_userService.Count(nameFree));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto user, CancellationToken cancellationToken)
        {
            var userNew = await _userService.CreateAsync(user, cancellationToken);
            return Created($"/User/{userNew.Id}", userNew);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user, int id, CancellationToken cancellationToken)
        {
            var userUpdt = await _userService.UpdateAsync(id, user, cancellationToken);
            return Ok(userUpdt);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}