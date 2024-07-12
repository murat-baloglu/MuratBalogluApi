using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class News : BaseEntity
    {
        public string Title { get; set; }

        public string Link { get; set; }

        //Navigation Properties
        public NewsImageFile? NewsImageFile { get; set; }
    }
}
