namespace MuratBaloglu.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int accessTokenLifeTime); //Second olarak
        string CreateRefreshToken();
    }
}
