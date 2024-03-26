using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class Speciality : BaseEntity
    {
        public Speciality()
        {
            SpecialityImageFiles = new HashSet<SpecialityImageFile>();
        }

        public string Title { get; set; }

        public string Context { get; set; }

        public string CardContext { get; set; }

        //Title property si düzenlenip bu property e atama yapılıyor.
        public string DetailUrl { get; set; }


        //Navigation Properties
        public ICollection<SpecialityImageFile> SpecialityImageFiles { get; set; }
    }
}
