using MediatR;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.DTOs.User;

namespace MuratBaloglu.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.CreateAsync(new DTOs.User.CreateUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                //FullName = request.FullName,
                //UserName = request.UserName, UserName ile Email in aynı olmasını istediğimiz için. İleride UserName i ve Emaili ayırmak istersek tekrar bu satırı aktif et alttakini yorum satırı yap.
                UserName = request.Email,
                Email = request.Email,
                Password = request.Password,
                PasswordConfirmed = request.PasswordConfirmed
            });

            return new CreateUserCommandResponse
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };
        }
    }
}
