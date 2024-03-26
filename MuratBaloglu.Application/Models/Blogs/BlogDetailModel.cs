using MuratBaloglu.Application.Models.Common;

namespace MuratBaloglu.Application.Models.Blogs
{
    public class BlogDetailModel
    {
        public List<TitleAndDetailUrlModel>? TitlesAndDetailUrls { get; set; }
        public string? Context { get; set; }
        public string? Title { get; set; }
    }
}
