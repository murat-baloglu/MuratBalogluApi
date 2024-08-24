using MediatR;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.Models.Roles;

namespace MuratBaloglu.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            List<RoleModel> roles = await _roleService.GetRolesAsync();
            return new GetRolesQueryResponse() { Roles = roles };
        }
    }
}
