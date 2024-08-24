using MuratBaloglu.Domain.Entities.Common;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Domain.Entities
{
    public class Endpoint : BaseEntity
    {
        public Endpoint()
        {
            AppRoles = new HashSet<AppRole>();
        }

        public Guid MenuId { get; set; }
        public string ActionType { get; set; }
        public string HttpType { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }

        //Navigation Properties
        public Menu Menu { get; set; }
        public ICollection<AppRole> AppRoles { get; set; }
    }
}
