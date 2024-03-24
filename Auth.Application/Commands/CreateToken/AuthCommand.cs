using Auth.Application.Dto;
using MediatR;

namespace Auth.Application.Commands.CreateToken
{
    public class AuthCommand: IRequest<JwtTokenDto>
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
