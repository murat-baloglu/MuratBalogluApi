using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Commands.AppUser.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserAsync(new DTOs.User.UpdateUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Id = request.Id,
            });

            return new UpdateUserCommandResponse() { Message = result.Message, Succeeded = result.Succeeded };
        }
    }
}
