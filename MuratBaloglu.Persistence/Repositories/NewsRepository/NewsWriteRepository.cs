using MuratBaloglu.Application.Repositories.NewsRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.NewsRepository
{
    public class NewsWriteRepository : WriteRepository<News>, INewsWriteRepository
    {
        public NewsWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
