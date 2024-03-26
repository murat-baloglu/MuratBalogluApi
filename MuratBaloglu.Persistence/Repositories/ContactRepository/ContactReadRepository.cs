using MuratBaloglu.Application.Repositories.ContactRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.ContactRepository
{
    public class ContactReadRepository : ReadRepository<Contact>, IContactReadRepository
    {
        public ContactReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
