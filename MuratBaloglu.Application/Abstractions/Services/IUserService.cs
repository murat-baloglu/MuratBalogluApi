using MuratBaloglu.Application.DTOs.User;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate); //Second olarak
    }
}
