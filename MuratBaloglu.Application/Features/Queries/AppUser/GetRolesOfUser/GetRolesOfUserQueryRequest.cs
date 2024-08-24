using MediatR;

namespace MuratBaloglu.Application.Features.Queries.AppUser.GetRolesOfUser
{
    public class GetRolesOfUserQueryRequest : IRequest<GetRolesOfUserQueryResponse>
    {
        public string UserId { get; set; }
    }
}