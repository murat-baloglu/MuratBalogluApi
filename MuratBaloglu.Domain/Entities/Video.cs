using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class Video : BaseEntity
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
