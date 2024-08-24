using MediatR;

namespace MuratBaloglu.Application.Features.Queries.AuthorizationEndpoint.GetRolesOfEndpoint
{
    public class GetRolesOfEndpointQueryRequest : IRequest<GetRolesOfEndpointQueryResponse>
    {
        public string Code { get; set; }
        public string MenuName { get; set; }
    }
}
