using MuratBaloglu.Application.Repositories.WorkingHourRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.WorkingHourRepository
{
    public class WorkingHourReadRepository : ReadRepository<WorkingHour>, IWorkingHourReadRepository
    {
        public WorkingHourReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
