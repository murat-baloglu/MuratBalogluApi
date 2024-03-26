using MuratBaloglu.Application.Repositories.PatientCommentRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.PatientCommentRepository
{
    public class PatientCommentReadRepository : ReadRepository<PatientComment>, IPatientCommentReadRepository
    {
        public PatientCommentReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
