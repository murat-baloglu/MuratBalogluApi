namespace MuratBaloglu.Domain.Entities
{
    public class SpecialityImageFile : File
    {
        public bool IsSpecialityCardImage { get; set; }
        public Guid SpecialityId { get; set; }

        //Navigation Properties
        public Speciality Speciality { get; set; }
    }
}
