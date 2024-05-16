using MuratBaloglu.Application.Abstractions.Services.Authentications;

namespace MuratBaloglu.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
    }
}
