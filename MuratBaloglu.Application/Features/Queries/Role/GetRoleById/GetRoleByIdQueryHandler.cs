using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Queries.Role.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _roleService.GetRoleByIdAsync(request.Id);
            return new GetRoleByIdQueryResponse { Id = data.id, Name = data.name };
        }
    }
}
