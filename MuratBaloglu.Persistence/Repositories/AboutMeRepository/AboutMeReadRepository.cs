using MuratBaloglu.Application.Repositories.AboutMeRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.AboutMeRepository
{
    public class AboutMeReadRepository : ReadRepository<AboutMe>, IAboutMeReadRepository
    {
        public AboutMeReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
