namespace MuratBaloglu.Application.Features.Commands.Role.CreateRole
{
    public class CreateRoleCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string? MessageCode { get; set; }
        public string? MessageDescription { get; set; }
    }
}