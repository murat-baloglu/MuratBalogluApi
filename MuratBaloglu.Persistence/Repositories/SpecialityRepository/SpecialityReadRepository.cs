using MuratBaloglu.Application.Repositories.SpecialityRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SpecialityRepository
{
    public class SpecialityReadRepository : ReadRepository<Speciality>, ISpecialityReadRepository
    {
        public SpecialityReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
