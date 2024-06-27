using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class SpecialityCategory : BaseEntity
    {
        public SpecialityCategory()
        {
            Specialties = new HashSet<Speciality>();
        }

        public string Name { get; set; }

        //Name property si düzenlenip bu property e atama yapılıyor.
        public string CategoryUrl { get; set; }


        //Navigation Properties
        public ICollection<Speciality> Specialties { get; set; }
    }
}
