namespace MuratBaloglu.Application.Models.News
{
    public class NewsWithCardImageModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? NewsImageFileId { get; set; }
        public string? FileName { get; set; }
        public string? Path { get; set; }
    }
}
