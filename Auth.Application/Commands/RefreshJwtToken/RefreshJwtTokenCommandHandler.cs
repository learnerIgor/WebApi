using Auth.Application.Dto;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;

namespace Auth.Application.Commands.RefreshJwtToken
{
    public class RefreshJwtTokenCommandHandler: IRequestHandler<RefreshJwtTokenCommand, JwtTokenDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        public RefreshJwtTokenCommandHandler(IRepository<ApplicationUser> userRepository, IConfiguration configuration, IRepository<RefreshToken> refreshTokenRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<JwtTokenDto> Handle(RefreshJwtTokenCommand request, CancellationToken cancellationToken)
        {
            //проверяем существует ли такой refreshToken
            var refreshTokenFromDb = await _refreshTokenRepository.SingleOrDefaultAsync(i => i.Id == request.RefreshToken, cancellationToken);
            if (refreshTokenFromDb is null)
            {
                Log.Error($"There isn't user in DB with such refreshToken");
                throw new ForbiddenException();
            }

            var user = await _userRepository.SingleAsync(l => l.Id == refreshTokenFromDb.ApplicationUserId, cancellationToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Login),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var dateExpires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Expires"]!));
            var tokenDescriptor = new JwtSecurityToken(_configuration["Jwt:Issuer"]!, _configuration["Jwt:Audience"]!, claims, expires: dateExpires, signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return new JwtTokenDto
            {
                JwtToken = token,
                RefreshToken = refreshTokenFromDb.Id,
                Expires = dateExpires
            };
        }
    }
}
