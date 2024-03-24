using MediatR;
using Users.Application.Dto;

namespace Users.Application.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest<GetUserDto>
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; } = default!;
        public UpdatePasswordCommand(int id, UpdatePasswordPayLoad updatePasswordPayLoad)
        {
            PasswordHash = updatePasswordPayLoad.PasswordHash;
            Id = id;
        }
    }
}
