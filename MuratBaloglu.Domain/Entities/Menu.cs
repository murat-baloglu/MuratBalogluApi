using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public Menu()
        {
            Endpoints = new HashSet<Endpoint>();
        }

        public string Name { get; set; }

        //Navigation Properties
        public ICollection<Endpoint> Endpoints { get; set; }
    }
}
