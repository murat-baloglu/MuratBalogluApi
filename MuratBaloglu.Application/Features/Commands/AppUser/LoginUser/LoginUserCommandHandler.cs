using MediatR;
using Microsoft.Extensions.Configuration;
using MuratBaloglu.Application.Abstractions.Services.Authentications;
using MuratBaloglu.Application.DTOs;

namespace MuratBaloglu.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IInternalAuthentication _authService;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(IInternalAuthentication authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Token token = await _authService.LoginAsync(request.UserNameOrEmail, request.Password, _configuration.GetValue<int>("Token:AccessTokenLifeTime"));
                return new LoginUserSuccessCommandResponse { Token = token };
            }
            catch (Exception ex)
            {
                return new LoginUserErrorCommandResponse { Message = ex.Message };
            }

            //Token token = await _authService.LoginAsync(request.UserNameOrEmail, request.Password, 60);
            //return new LoginUserSuccessCommandResponse { Token = token };            
        }
    }
}
