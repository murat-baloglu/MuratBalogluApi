using MuratBaloglu.Application.Models.Roles;

namespace MuratBaloglu.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryResponse
    {
        public List<RoleModel> Roles { get; set; }
    }
}