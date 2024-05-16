using MediatR;
using Microsoft.AspNetCore.Mvc;
using MuratBaloglu.Application.Features.Commands.AppUser.CreateUser;
using MuratBaloglu.Application.Features.Commands.AppUser.LoginUser;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
