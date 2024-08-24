using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Features.Commands.Role.CreateRole;
using MuratBaloglu.Application.Features.Commands.Role.DeleteRole;
using MuratBaloglu.Application.Features.Queries.Role.GetRoles;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = "Roller", ActionType = ActionType.Reading, Definition = "Rolleri Getirme")]
        public async Task<IActionResult> GetRolesAsync()
        {
            try
            {
                GetRolesQueryResponse response = await _mediator.Send(new GetRolesQueryRequest());
                return Ok(response.Roles);
            }
            catch
            {
                return BadRequest(new { Message = "Roller listelenirken bir hata ile karşılaşıldı." });
            }
        }

        //[HttpGet("{Id}")]
        //[AuthorizeDefinition(Menu = "Roller", ActionType = ActionType.Reading, Definition = "Id İle Role Getirme")]
        //public async Task<IActionResult> GetRoleByIdAsync([FromRoute] GetRoleByIdQueryRequest request)
        //{
        //    GetRoleByIdQueryResponse response = await _mediator.Send(request);
        //    return Ok(response);
        //}

        [HttpPost]
        [AuthorizeDefinition(Menu = "Roller", ActionType = ActionType.Writing, Definition = "Role Ekleme")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleCommandRequest request)
        {
            CreateRoleCommandResponse response = await _mediator.Send(request);
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        //[HttpPut("{Id}")]
        //[AuthorizeDefinition(Menu = "Roller", ActionType = ActionType.Updating, Definition = "Role Güncelleme")]
        //public async Task<IActionResult> UpdateRoleAsync([FromBody, FromRoute] UpdateRoleCommandRequest request)
        //{
        //    UpdateRoleCommandResponse response = await _mediator.Send(request);
        //    return Ok(response);
        //}

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(Menu = "Roller", ActionType = ActionType.Deleting, Definition = "Role Silme")]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute] DeleteRoleCommandRequest request)
        {
            DeleteRoleCommandResponse response = await _mediator.Send(request);
            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
