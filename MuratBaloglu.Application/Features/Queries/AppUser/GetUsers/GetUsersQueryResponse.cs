using MuratBaloglu.Application.DTOs.User;

namespace MuratBaloglu.Application.Features.Queries.AppUser.GetUsers
{
    public class GetUsersQueryResponse
    {
        public List<UserDto> Users { get; set; }
    }
}