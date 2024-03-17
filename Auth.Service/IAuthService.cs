using Auth.Service.Dto;

namespace Auth.Service
{
    public interface IAuthService
    {
        Task<JwtTokenDto> GetJwtTokenAsync(AuthDto authDto, CancellationToken cancellationToken);
        Task<JwtTokenDto> CreateJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}