using Microsoft.AspNetCore.Identity;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.Abstractions.Token;
using MuratBaloglu.Application.DTOs;
using MuratBaloglu.Application.Exceptions;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            AppUser? user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userNameOrEmail);

            if (user == null)
                throw new NotFoundUserException();

            //Artık biliyoruzki database de böyle bir user var. Bu aşamada artık Authentication(Doğrulama) işlemi yapıyoruz.
            //CheckPasswordSignInAsync bu methodu parametre olarak vereceğimiz AppUser türündeki nesnenin yine parametre olarak vereceğimiz
            //password ile doğrulanıp doğrulanmadığını yani otantike olup olmadığının sonucunu SignInResult olarak geri döndürmektedir.
            //Yani diyoruzki bu user, bu password ile doğrulanoyor mu bir kontrol et.
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded) //Authentication basarili! (Doğrulama süreci burada bitiyor.)
            {
                //Yetkileri belirlememiz gerekiyor! Authorization (Yetkilendirme işlemi burada başlıyor.) Token üretilir ve bu değer geriye döndürülür.
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return token;
            }

            throw new AuthenticationErrorException();
        }
    }
}
