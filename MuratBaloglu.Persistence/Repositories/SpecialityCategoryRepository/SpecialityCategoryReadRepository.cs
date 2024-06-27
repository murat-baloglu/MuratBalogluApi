using MuratBaloglu.Application.Repositories.SpecialityCategoryRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SpecialityCategoryRepository
{
    public class SpecialityCategoryReadRepository : ReadRepository<SpecialityCategory>, ISpecialityCategoryReadRepository
    {
        public SpecialityCategoryReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
