using MediatR;
using Users.Application.Dto;

namespace Users.Application.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<GetUserDto>
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public UpdateUserCommand(int id, UpdateUserCommandPayLoad updateUserCommandPayLoad)
        {
            Id = id;
            Login = updateUserCommandPayLoad.Login;
        }
    }
}
