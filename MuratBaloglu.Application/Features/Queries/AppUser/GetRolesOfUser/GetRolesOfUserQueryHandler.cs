using MediatR;
using MuratBaloglu.Application.Abstractions.Services;

namespace MuratBaloglu.Application.Features.Queries.AppUser.GetRolesOfUser
{
    public class GetRolesOfUserQueryHandler : IRequestHandler<GetRolesOfUserQueryRequest, GetRolesOfUserQueryResponse>
    {
        private readonly IUserService _userService;

        public GetRolesOfUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetRolesOfUserQueryResponse> Handle(GetRolesOfUserQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string[] userRoles = await _userService.GetRolesOfUserAsync(request.UserId);
                return new GetRolesOfUserQueryResponse { UserRoles = userRoles };
            }
            catch (Exception ex)
            {
                return new GetRolesOfUserQueryResponse { Message = ex.Message };
            }
        }
    }
}
