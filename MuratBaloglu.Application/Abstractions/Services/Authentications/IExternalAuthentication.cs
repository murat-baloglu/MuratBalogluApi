using MuratBaloglu.Application.DTOs;

namespace MuratBaloglu.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthentication
    {
        //Sosyal Medya hesapları üzerinden üye olma ve giriş yapma işlemleri yapıldığında aşağıdaki yorum satırlarını kaldır.
        //Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime);
        //Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
    }
}
