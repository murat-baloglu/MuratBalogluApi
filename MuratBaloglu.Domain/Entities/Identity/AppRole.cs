using Microsoft.AspNetCore.Identity;

namespace MuratBaloglu.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        //Navigation Properties
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
