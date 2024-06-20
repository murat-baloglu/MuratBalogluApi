using Microsoft.AspNetCore.Identity;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.DTOs.User;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
                response.Message = "Kayıt olma işleminiz başarılı bir şekilde gerçekleşmiştir. Üyeliğiniz onaylandıktan sonra panele giriş yapabilirsiniz.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
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
