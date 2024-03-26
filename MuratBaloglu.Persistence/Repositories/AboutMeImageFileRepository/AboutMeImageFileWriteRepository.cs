using MuratBaloglu.Application.Repositories.AboutMeImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.AboutMeImageFileRepository
{
    public class AboutMeImageFileWriteRepository : WriteRepository<AboutMeImageFile>, IAboutMeImageFileWriteRepository
    {
        public AboutMeImageFileWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
