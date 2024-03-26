namespace MuratBaloglu.Domain.Entities
{
    public class AboutMeImageFile : File
    {
        public Guid AboutMeId { get; set; }

        //Navigation Property
        public AboutMe? AboutMe { get; set; }
    }
}
