using MediatR;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.DTOs;

namespace MuratBaloglu.Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        private readonly IAuthService _authService;

        public RefreshTokenLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Token token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
                return new RefreshTokenLoginSuccessCommandResponse { Token = token };
            }
            catch (Exception ex)
            {
                return new RefreshTokenLoginErrorCommandResponse { Message = ex.Message };
            }

            //Token token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
            //return new RefreshTokenLoginCommandResponse { Token = token };
        }
    }
}
