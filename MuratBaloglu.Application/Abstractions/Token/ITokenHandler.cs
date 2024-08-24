using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int accessTokenLifeTime, AppUser appUser); //Second olarak
        string CreateRefreshToken();
    }
}
