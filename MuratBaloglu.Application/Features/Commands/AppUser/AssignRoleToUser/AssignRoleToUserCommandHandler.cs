using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Commands.AppUser.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommandRequest, AssignRoleToUserCommandResponse>
    {
        private readonly IUserService _userService;

        public AssignRoleToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.AssignRoleToUserAsync(request.UserId, request.Roles);
                return new AssignRoleToUserCommandResponse();
            }
            catch (Exception ex)
            {
                return new AssignRoleToUserCommandResponse() { Message = ex.Message };
            }
        }
    }
}
