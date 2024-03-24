using Auth.Application.Commands.CreateToken;
using Auth.Application.Commands.RefreshJwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [AllowAnonymous]
        [HttpPost("/CreateJwtToken")]
        public async Task<IActionResult> CreateJwtToken(
            [FromBody] AuthCommand authDto,
            IMediator mediator,  
            CancellationToken cancellationToken)
        {
            var createJwt = await mediator.Send(authDto, cancellationToken);
            return Ok(createJwt);
        }

        [AllowAnonymous]
        [HttpPost("/CreateJwtTokenByRefreshTokenAsync")]
        public async Task<IActionResult> CreateJwtTokenByRefreshTokenAsync(
            [FromQuery] RefreshJwtTokenCommand refreshJwtTokenCommand,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var createRefreshJwt = await mediator.Send(refreshJwtTokenCommand, cancellationToken);
            return Ok(createRefreshJwt);
        }
    }
}
