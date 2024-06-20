using MuratBaloglu.Application.DTOs;

namespace MuratBaloglu.Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandResponse
    {
    }

    public class RefreshTokenLoginSuccessCommandResponse : RefreshTokenLoginCommandResponse
    {
        public Token Token { get; set; }
    }

    public class RefreshTokenLoginErrorCommandResponse : RefreshTokenLoginCommandResponse
    {
        public string Message { get; set; }
    }

    //public class RefreshTokenLoginCommandResponse
    //{
    //    public Token Token { get; set; }
    //}    
}
