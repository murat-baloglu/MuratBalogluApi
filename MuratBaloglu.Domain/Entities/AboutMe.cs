using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class AboutMe : BaseEntity
    {
        public string? Context { get; set; }
        public string? HomeContext { get; set; }

        public AboutMeImageFile? AboutMeImageFile { get; set; }
    }
}
