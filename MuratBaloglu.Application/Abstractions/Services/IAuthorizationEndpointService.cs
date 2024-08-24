namespace MuratBaloglu.Application.Abstractions.Services
{
    public interface IAuthorizationEndpointService
    {
        public Task AssignRoleToEndpointAsync(string[] roles, string menuName, string code, Type type);
        public Task<List<string>> GetRolesOfEndpointAsync(string code, string menuName);
    }
}
