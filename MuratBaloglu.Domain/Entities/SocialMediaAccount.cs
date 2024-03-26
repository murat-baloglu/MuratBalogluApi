using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class SocialMediaAccount : BaseEntity
    {
        public string? YouTube { get; set; }
        public string? LinkedIn { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? X { get; set; }
    }
}
