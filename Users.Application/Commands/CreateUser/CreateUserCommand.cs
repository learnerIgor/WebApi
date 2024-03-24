using MediatR;
using Users.Application.Dto;

namespace Users.Application.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<GetUserDto>
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
