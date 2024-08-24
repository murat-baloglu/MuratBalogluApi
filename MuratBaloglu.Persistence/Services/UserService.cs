using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.DTOs.User;
using MuratBaloglu.Application.Exceptions;
using MuratBaloglu.Application.Repositories.EndpointRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEndpointReadRepository _endpointReadRepository;

        public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            }, model.Password);

            CreateUserResponse response = new CreateUserResponse()
            {
                Succeeded = result.Succeeded
            };

            if (result.Succeeded)
                response.Message = "Kayıt olma işleminiz başarılı bir şekilde gerçekleşmiştir. Giriş yapabilirsiniz.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();

            return users.Select(user => new UserDto()
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                //TwoFactorEnabled = user.TwoFactorEnabled
            }).ToList();
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUser model)
        {
            AppUser? user = await _userManager.FindByIdAsync(model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            IdentityResult result = await _userManager.UpdateAsync(user);

            UpdateUserResponse response = new UpdateUserResponse() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Güncelleme işlemi gerçekleşmiştir";
            else
            {
                IdentityError error = result.Errors.First();
                response.Message = error.Description;
            }

            return response;
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);

            IdentityResult result = await _userManager.DeleteAsync(user);

            DeleteUserResponse response = new DeleteUserResponse() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Silme işlemi gerçekleşmiştir";
            else
            {
                IdentityError error = result.Errors.First();
                response.Message = error.Description;
            }

            return response;
        }

        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, roles);
            }
            else
                throw new NotFoundUserException("Kullanıcıya rollerini atama yaparken bir hata ile karşılaşıldı.");
        }

        public async Task<string[]> GetRolesOfUserAsync(string userId)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }

            throw new NotFoundUserException("Kullanıcının rollerini getirirken bir hata ile karşılaşıldı.");
            //return null; 
            //return Array.Empty<string>();
        }

        private async Task<string[]> GetRolesOfUserByUserNameAsync(string userName)
        {
            AppUser? user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }

            return Array.Empty<string>();
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesOfUserByUserNameAsync(name);

            if (!userRoles.Any()) return false;

            if (userRoles.Contains("Admin")) return true;

            Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.AppRoles).FirstOrDefaultAsync(e => e.Code == code);
            if (endpoint == null) return false;

            var endpointRoles = endpoint.AppRoles.Select(r => r.Name);

            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                {
                    if (userRole == endpointRole)
                        return true;
                }
            }

            return false;

            //Bunuda kullanabiliriz 2. yol olarak
            //bool hasRole = false;
            //var endpointRoles = endpoint.AppRoles.Select(r => r.Name);

            //foreach (var userRole in userRoles)
            //{
            //    if (!hasRole)
            //    {
            //        foreach (var endpointRole in endpointRoles)
            //        {
            //            if (userRole == endpointRole)
            //            {
            //                hasRole = true;
            //                break;
            //            }
            //        }
            //    }
            //    else
            //        break;
            //}

            //return hasRole;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
            await _userManager.UpdateAsync(user);

            //if (user != null)
            //{
            //    user.RefreshToken = refreshToken;
            //    user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
            //    await _userManager.UpdateAsync(user);
            //}
            //else
            //    throw new NotFoundUserException("Refresh Token hatası. Bu kullanıcı ID ye sahip bir hesap bulamıyoruz.");
        }
    }
}
