using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class PatientComment : BaseEntity
    {
        public string PatientName { get; set; } //Hasta Adı
        public string Disease { get; set; } //Hastalık
        public string PatientReview { get; set; } //Hasta Yorumu/Görüşü
    }
}
