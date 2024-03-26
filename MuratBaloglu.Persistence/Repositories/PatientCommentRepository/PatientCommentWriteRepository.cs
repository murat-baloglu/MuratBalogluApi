using MuratBaloglu.Application.Repositories.PatientCommentRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.PatientCommentRepository
{
    public class PatientCommentWriteRepository : WriteRepository<PatientComment>, IPatientCommentWriteRepository
    {
        public PatientCommentWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
