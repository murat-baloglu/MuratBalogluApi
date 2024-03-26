using MuratBaloglu.Application.Repositories.WorkingHourRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.WorkingHourRepository
{
    public class WorkingHourWriteRepository : WriteRepository<WorkingHour>, IWorkingHourWriteRepository
    {
        public WorkingHourWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
