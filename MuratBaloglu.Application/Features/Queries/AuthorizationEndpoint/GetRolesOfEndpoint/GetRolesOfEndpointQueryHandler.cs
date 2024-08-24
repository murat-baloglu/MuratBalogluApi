using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Queries.AuthorizationEndpoint.GetRolesOfEndpoint
{
    public class GetRolesOfEndpointQueryHandler : IRequestHandler<GetRolesOfEndpointQueryRequest, GetRolesOfEndpointQueryResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;

        public GetRolesOfEndpointQueryHandler(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }

        public async Task<GetRolesOfEndpointQueryResponse> Handle(GetRolesOfEndpointQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                List<string> endpointRoles = await _authorizationEndpointService.GetRolesOfEndpointAsync(request.Code, request.MenuName);
                return new GetRolesOfEndpointQueryResponse { EndpointRoles = endpointRoles };
            }
            catch (Exception ex)
            {
                return new GetRolesOfEndpointQueryResponse { Message = ex.Message };
            }
        }
    }
}
