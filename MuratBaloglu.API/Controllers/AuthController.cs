using MediatR;
using Microsoft.AspNetCore.Mvc;
using MuratBaloglu.Application.Features.Commands.AppUser.LoginUser;
using MuratBaloglu.Application.Features.Commands.AppUser.RefreshTokenLogin;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);

            if (response.GetType().Equals(typeof(LoginUserSuccessCommandResponse)))
                return Ok(response);

            return StatusCode(500, response);

            //LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);
            //return Ok(response);            
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            if (response.GetType().Equals(typeof(RefreshTokenLoginSuccessCommandResponse)))
                return Ok(response);

            return StatusCode(500, response);

            //RefreshTokenLoginCommandResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            //return Ok(response);
        }
    }
}
