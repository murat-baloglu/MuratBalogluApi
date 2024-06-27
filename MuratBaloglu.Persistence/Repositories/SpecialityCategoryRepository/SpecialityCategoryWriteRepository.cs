using MuratBaloglu.Application.Repositories.SpecialityCategoryRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SpecialityCategoryRepository
{
    public class SpecialityCategoryWriteRepository : WriteRepository<SpecialityCategory>, ISpecialityCategoryWriteRepository
    {
        public SpecialityCategoryWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
