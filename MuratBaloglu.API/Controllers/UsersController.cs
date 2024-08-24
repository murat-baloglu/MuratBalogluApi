using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Features.Commands.AppUser.AssignRoleToUser;
using MuratBaloglu.Application.Features.Commands.AppUser.CreateUser;
using MuratBaloglu.Application.Features.Commands.AppUser.DeleteUser;
using MuratBaloglu.Application.Features.Commands.AppUser.UpdateUser;
using MuratBaloglu.Application.Features.Queries.AppUser.GetRolesOfUser;
using MuratBaloglu.Application.Features.Queries.AppUser.GetUsers;

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

        [HttpPut("update-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Updating, Definition = "Kullanıcı Güncelleme")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserCommandRequest request)
        {
            UpdateUserCommandResponse response = await _mediator.Send(request);
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Deleting, Definition = "Kullanıcı Silme")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] DeleteUserCommandRequest request)
        {
            DeleteUserCommandResponse response = await _mediator.Send(request);
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        //[HttpPost("update-password")]
        //public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest request)
        //{
        //    UpdatePasswordCommandResponse response = await _mediator.Send(UpdatePasswordCommandRequest);

        //    return Ok(response);
        //}

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Kullanıcıları Getirme")]
        public async Task<IActionResult> GetUsersAsync()
        {
            try
            {
                GetUsersQueryResponse response = await _mediator.Send(new GetUsersQueryRequest());
                return Ok(response.Users);
            }
            catch
            {
                return BadRequest(new { Message = "Kullanıcılar listelenirken bir hata ile karşılaşıldı." });
            }
        }

        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Writing, Definition = "Kullanıcıya Rol Atama")]
        public async Task<IActionResult> AssignRoleToUserAsync(AssignRoleToUserCommandRequest request)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(request);
            if (response.Message is null)
                return Ok(response);

            return StatusCode(500, response);
        }

        [HttpGet("get-roles-of-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Kullanıcının Rollerini Getirme")]
        public async Task<IActionResult> GetRolesOfUserAsync([FromRoute] GetRolesOfUserQueryRequest request)
        {
            GetRolesOfUserQueryResponse response = await _mediator.Send(request);
            if (response.Message is null)
                return Ok(response);

            return StatusCode(500, response);
        }
    }
}
