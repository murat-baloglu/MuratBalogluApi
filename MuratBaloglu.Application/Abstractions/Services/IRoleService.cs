using MuratBaloglu.Application.DTOs.Role;
using MuratBaloglu.Application.Models.Roles;

namespace MuratBaloglu.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<List<RoleModel>> GetRolesAsync();
        //IDictionary<string, string?> GetAllRoles();
        Task<(string id, string name)> GetRoleByIdAsync(string id);
        Task<CreateRoleResponse> CreateRoleAsync(string name);
        Task<DeleteRoleResponse> DeleteRoleAsync(string id);
        //Task<bool> UpdateRoleAsync(string id, string name);
    }
}
