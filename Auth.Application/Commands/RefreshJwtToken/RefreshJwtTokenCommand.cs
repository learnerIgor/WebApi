using Auth.Application.Dto;
using MediatR;

namespace Auth.Application.Commands.RefreshJwtToken
{
    public class RefreshJwtTokenCommand: IRequest<JwtTokenDto>
    {
        public string RefreshToken { get; set; } = default!;
    }
}
