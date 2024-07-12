namespace MuratBaloglu.Domain.Entities
{
    public class NewsImageFile : File
    {
        public Guid NewsId { get; set; }

        //Navigation Property
        public News? News { get; set; }
    }
}
