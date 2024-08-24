using MediatR;

namespace MuratBaloglu.Application.Features.Commands.AppUser.DeleteUser
{
    public class DeleteUserCommandRequest : IRequest<DeleteUserCommandResponse>
    {
        public string Id { get; set; }
    }
}
