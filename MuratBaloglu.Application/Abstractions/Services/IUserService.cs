using MuratBaloglu.Application.DTOs.User;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate); //Second olarak
        //Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<List<UserDto>> GetUsersAsync();
        Task AssignRoleToUserAsync(string userId, string[] roles);
        Task<string[]> GetRolesOfUserAsync(string userId);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
        Task<DeleteUserResponse> DeleteUserAsync(string id);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUser model);
    }
}
