using MuratBaloglu.Application.Repositories.ContactRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.ContactRepository
{
    public class ContactWriteRepository : WriteRepository<Contact>, IContactWriteRepository
    {
        public ContactWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
