using MuratBaloglu.Application.DTOs.User;

namespace MuratBaloglu.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
    }
}
