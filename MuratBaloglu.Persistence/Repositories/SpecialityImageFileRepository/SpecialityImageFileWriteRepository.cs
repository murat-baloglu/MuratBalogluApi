using MuratBaloglu.Application.Repositories.SpecialityImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SpecialityImageFileRepository
{
    public class SpecialityImageFileWriteRepository : WriteRepository<SpecialityImageFile>, ISpecialityImageFileWriteRepository
    {
        public SpecialityImageFileWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
