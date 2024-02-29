using Common.Domain;
using Microsoft.AspNetCore.Mvc;
using Users.Service;

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
        [HttpGet("/users")]
        public IActionResult ListUsers(int? offset, string? nameFree, int? limit)
        {
            var users = _userService.GetListUsers(offset, nameFree, limit);
            return Ok(users);
        }
        [HttpGet("/users/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetIdUser(id);
            if (user != null)
                return Ok(user);
            return NotFound($"{id}");
        }
        [HttpPost("/users")]
        public IActionResult CreateUser(User user)
        {
            var userNew = _userService.Create(user);
            return Created($"/users/{userNew.Id}", userNew);
        }
        [HttpPut("/users/{id}")]
        public IActionResult UpdateUser(User user, int id)
        {
            user.Id = id;
            var userUpdt = _userService.Update(user);
            if (userUpdt != null)
                return Ok(userUpdt);
            return NotFound($"{id}");
        }
        [HttpDelete("/users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userDlt = _userService.Delete(id);
            if (userDlt)
                return Ok(userDlt);
            return NotFound($"{id}");
        }
    }
}