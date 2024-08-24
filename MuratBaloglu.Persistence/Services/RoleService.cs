using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.DTOs.Role;
using MuratBaloglu.Application.Models.Roles;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Persistence.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<CreateRoleResponse> CreateRoleAsync(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new AppRole() { Name = name });

            CreateRoleResponse response = new CreateRoleResponse()
            {
                Succeeded = result.Succeeded
            };

            if (result.Succeeded)
            {
                response.Message = "Yükleme işlemi gerçekleşmiştir";
                response.MessageCode = "Başarılı";
            }
            else
            {
                IdentityError error = result.Errors.First();
                response.Message = $"{error.Code} - {error.Description}";
                response.MessageCode = error.Code;
                response.MessageDescription = error.Description;
            }

            return response;
        }

        public async Task<DeleteRoleResponse> DeleteRoleAsync(string id)
        {
            IdentityResult result = await _roleManager.DeleteAsync(new AppRole() { Id = Guid.Parse(id) });

            DeleteRoleResponse response = new DeleteRoleResponse()
            {
                Succeeded = result.Succeeded
            };

            if (result.Succeeded)
                response.Message = "Silme işlemi gerçekleşmiştir";
            else
            {
                IdentityError error = result.Errors.First();
                response.Message = error.Description;
            }

            return response;
        }

        //public async Task<bool> UpdateRoleAsync(string id, string name)
        //{
        //    IdentityResult result = await _roleManager.UpdateAsync(new AppRole { Id = Guid.Parse(id), Name = name });
        //    return result.Succeeded;
        //}

        public async Task<List<RoleModel>> GetRolesAsync()
        {
            return await _roleManager.Roles.Select(r => new RoleModel() { Id = r.Id.ToString(), Name = r.Name }).ToListAsync();
        }

        //public IDictionary<string, string?> GetAllRoles()
        //{
        //    return _roleManager.Roles.ToDictionary(role => role.Id.ToString(), role => role.Name);
        //}

        public async Task<(string id, string name)> GetRoleByIdAsync(string id) //Bu methodu hiç bir yerde kullanmadık
        {
            string role = await _roleManager.GetRoleIdAsync(new AppRole { Id = Guid.Parse(id) });
            return (id, role);
        }
    }
}
