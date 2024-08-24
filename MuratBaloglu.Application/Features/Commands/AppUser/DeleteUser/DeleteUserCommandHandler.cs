using MediatR;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.DTOs.User;

namespace MuratBaloglu.Application.Features.Commands.AppUser.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommandRequest, DeleteUserCommandResponse>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
        {
            DeleteUserResponse result = await _userService.DeleteUserAsync(request.Id);

            return new DeleteUserCommandResponse() { Message = result.Message, Succeeded = result.Succeeded };
        }
    }
}
