using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public Blog()
        {
            BlogImageFiles = new HashSet<BlogImageFile>();
        }

        public string Title { get; set; }

        public string Context { get; set; }

        public string CardContext { get; set; }

        //Title property si düzenlenip bu property e atama yapılacak.
        public string DetailUrl { get; set; }


        //Navigation Properties
        public ICollection<BlogImageFile> BlogImageFiles { get; set; }
    }
}
