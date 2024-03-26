namespace MuratBaloglu.Domain.Entities
{
    public class BlogImageFile : File
    {
        public bool IsBlogCardImage { get; set; }
        public Guid BlogId { get; set; }

        //Navigation Properties
        public Blog Blog { get; set; }
    }
}
