using MuratBaloglu.Application.Repositories.SpecialityImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SpecialityImageFileRepository
{
    public class SpecialityImageFileReadRepository : ReadRepository<SpecialityImageFile>, ISpecialityImageFileReadRepository
    {
        public SpecialityImageFileReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
