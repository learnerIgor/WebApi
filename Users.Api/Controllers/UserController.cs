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
        public IActionResult ListUsers(int? offset, string? nameFree, int? limit)
        {
            var users = _userService.GetListUsers(offset, nameFree, limit);
            var count = _userService.Count(nameFree);
            HttpContext.Response.Headers.Append("x-Total-Count", count.ToString());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetIdUser(id);
            return Ok(user);
        }

        [HttpGet("UsersCount")]
        public IActionResult GetCount(string? nameFree)
        {
            return Ok(_userService.Count(nameFree));
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDto user)
        {
            var userNew = _userService.Create(user);
            return Created($"/User/{userNew.Id}", userNew);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(UpdateUserDto user, int id)
        {
            var userUpdt = _userService.Update(id, user);
            return Ok(userUpdt);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}