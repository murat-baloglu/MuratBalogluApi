using MuratBaloglu.Application.Repositories.AboutMeRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.AboutMeRepository
{
    public class AboutMeWriteRepository : WriteRepository<AboutMe>, IAboutMeWriteRepository
    {
        public AboutMeWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
