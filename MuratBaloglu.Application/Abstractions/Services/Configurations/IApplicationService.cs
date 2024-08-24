using MuratBaloglu.Application.DTOs.Configurations;

namespace MuratBaloglu.Application.Abstractions.Services.Configurations
{
    public interface IApplicationService
    {
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
