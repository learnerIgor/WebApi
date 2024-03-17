using Auth.Service;
using Auth.Service.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("/CreateJwtToken")]
        public async Task<IActionResult> CreateJwtToken(AuthDto authDto, CancellationToken cancellationToken)
        {
            var createJwt = await _authService.GetJwtTokenAsync(authDto, cancellationToken);
            return Ok(createJwt);
        }

        [AllowAnonymous]
        [HttpPost("/CreateJwtTokenByRefreshTokenAsync")]
        public async Task<IActionResult> CreateJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var createRefreshJwt = await _authService.CreateJwtTokenByRefreshTokenAsync(refreshToken, cancellationToken);
            return Ok(createRefreshJwt);
        }
    }
}
