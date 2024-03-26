using MuratBaloglu.Application.Repositories.SpecialityRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SpecialityRepository
{
    public class SpecialityWriteRepository : WriteRepository<Speciality>, ISpecialityWriteRepository
    {
        public SpecialityWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
