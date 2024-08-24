using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint
{
    public class AssignRoleToEndpointCommandHandler : IRequestHandler<AssignRoleToEndpointCommandRequest, AssignRoleToEndpointCommandResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;

        public AssignRoleToEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }

        public async Task<AssignRoleToEndpointCommandResponse> Handle(AssignRoleToEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _authorizationEndpointService.AssignRoleToEndpointAsync(request.Roles, request.Menu, request.Code, request.Type);
                return new AssignRoleToEndpointCommandResponse();
            }
            catch
            {
                return new AssignRoleToEndpointCommandResponse() { Message = "Endpointe rollerini atama yaparken bir hata ile karşılaşıldı." };
            }
        }
    }
}
