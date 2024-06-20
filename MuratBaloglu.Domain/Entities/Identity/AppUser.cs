using Microsoft.AspNetCore.Identity;

namespace MuratBaloglu.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => string.Join(" ", FirstName, LastName);
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenEndDate { get; set; }
    }
}
