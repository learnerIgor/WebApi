using Auth.Service.Dto;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Common.Service.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        public AuthService(IRepository<ApplicationUser> userRepository, IRepository<RefreshToken> refreshTokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<JwtTokenDto> GetJwtTokenAsync(AuthDto authDto, CancellationToken cancellationToken)
        {
            //проверяем есть ли у нас пользователь с таким логином
            var user = await _userRepository.SingleOrDefaultAsync(l => l.Login == authDto.Login.Trim(), cancellationToken);
            if (user is null)
            {
                Log.Error($"There isn't user in DB with such login {authDto.Login}");
                throw new NotFoundException($"There isn't user in DB with such login {authDto.Login}");
            }

            //проверяем пароль пользователя
            if (!PasswordHasher.VerifyPassword(authDto.Password, user.PasswordHash))
            {
                throw new ForbiddenException();
            }

            //если логин и пароль введены правильно, то мы создаем Jwt token для авторизации
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, authDto.Login),
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
            var refreshToken = await _refreshTokenRepository.AddAsync(new RefreshToken { ApplicationUserId = user.Id }, cancellationToken);

            return new JwtTokenDto
            {
                JwtToken = token,
                RefreshToken = refreshToken.Id,
                Expires = dateExpires
            };
        }

        public async Task<JwtTokenDto> CreateJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            //проверяем существует ли такой refreshToken
            var refreshTokenFromDb = await _refreshTokenRepository.SingleOrDefaultAsync(i => i.Id == refreshToken, cancellationToken);
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
