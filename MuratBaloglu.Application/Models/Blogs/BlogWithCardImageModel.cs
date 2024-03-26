namespace MuratBaloglu.Application.Models.Blogs
{
    public class BlogWithCardImageModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Context { get; set; }
        public string? CardContext { get; set; }
        public string? DetailUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? BlogImageFileId { get; set; }
        public string? FileName { get; set; }
        public string? Path { get; set; }
    }
}
