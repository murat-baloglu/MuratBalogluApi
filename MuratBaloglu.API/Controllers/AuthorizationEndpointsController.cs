using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuratBaloglu.Application.Consts;
using MuratBaloglu.Application.CustomAttributes;
using MuratBaloglu.Application.Enums;
using MuratBaloglu.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint;
using MuratBaloglu.Application.Features.Queries.AuthorizationEndpoint.GetRolesOfEndpoint;

namespace MuratBaloglu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("get-roles-of-endpoint")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints, ActionType = ActionType.Reading, Definition = "Endpointlerin Rollerini Getirme")]
        public async Task<IActionResult> GetRolesOfEndpointAsync(GetRolesOfEndpointQueryRequest request)
        {
            GetRolesOfEndpointQueryResponse response = await _mediator.Send(request);
            if (response.Message is null)
                return Ok(response.EndpointRoles);

            return StatusCode(500, response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints, ActionType = ActionType.Writing, Definition = "Endpointlere Rol Atama")]
        public async Task<IActionResult> AssignRoleToEndpointAsync(AssignRoleToEndpointCommandRequest request)
        {
            request.Type = typeof(Program);
            AssignRoleToEndpointCommandResponse response = await _mediator.Send(request);
            if (response.Message is null)
                return Ok(response);

            return StatusCode(500, response);
        }
    }
}
