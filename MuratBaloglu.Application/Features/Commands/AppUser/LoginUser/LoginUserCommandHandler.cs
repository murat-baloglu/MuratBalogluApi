using MediatR;
using MuratBaloglu.Application.Abstractions.Services.Authentications;
using MuratBaloglu.Application.DTOs;

namespace MuratBaloglu.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IInternalAuthentication _authService;

        public LoginUserCommandHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.LoginAsync(request.UserNameOrEmail, request.Password, 60);
            return new LoginUserSuccessCommandResponse { Token = token };
        }
    }
}
