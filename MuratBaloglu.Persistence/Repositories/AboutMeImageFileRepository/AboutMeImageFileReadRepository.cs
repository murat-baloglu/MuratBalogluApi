using MuratBaloglu.Application.Repositories.AboutMeImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.AboutMeImageFileRepository
{
    public class AboutMeImageFileReadRepository : ReadRepository<AboutMeImageFile>, IAboutMeImageFileReadRepository
    {
        public AboutMeImageFileReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
