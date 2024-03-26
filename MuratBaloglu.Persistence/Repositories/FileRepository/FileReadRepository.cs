using MuratBaloglu.Application.Repositories.FileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuratBaloglu.Persistence.Repositories.FileRepository
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
