using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public DeleteRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.DeleteRoleAsync(request.Id);
            return new DeleteRoleCommandResponse
            {
                Succeeded = result.Succeeded,
                Message = result.Message,
            };
        }
    }
}
