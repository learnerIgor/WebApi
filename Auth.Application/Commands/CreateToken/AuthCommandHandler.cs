using Auth.Application.Dto;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Application.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;

namespace Auth.Application.Commands.CreateToken
{
    public class AuthCommandHandler: IRequestHandler<AuthCommand, JwtTokenDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        public AuthCommandHandler(IRepository<ApplicationUser> userRepository, IConfiguration configuration, IRepository<RefreshToken> refreshTokenRepository) 
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<JwtTokenDto> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            //проверяем есть ли у нас пользователь с таким логином
            var user = await _userRepository.SingleOrDefaultAsync(l => l.Login == request.Login.Trim(), cancellationToken);
            if (user is null)
            {
                Log.Error($"There isn't user in DB with such login {request.Login}");
                throw new NotFoundException($"There isn't user in DB with such login {request.Login}");
            }

            //проверяем пароль пользователя
            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ForbiddenException();
            }

            //если логин и пароль введены правильно, то мы создаем Jwt token для авторизации
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.Login),
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
    }
}
