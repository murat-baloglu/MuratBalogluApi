using MuratBaloglu.Application.Models.Common;

namespace MuratBaloglu.Application.Models.Specialties
{
    public class SpecialityDetailModel
    {
        public List<TitleAndDetailUrlModel>? TitlesAndDetailUrls { get; set; }
        public string? Context { get; set; }
        public string? Title { get; set; }
    }
}
