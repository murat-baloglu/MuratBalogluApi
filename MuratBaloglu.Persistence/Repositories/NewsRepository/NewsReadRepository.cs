using MuratBaloglu.Application.Repositories.NewsRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.NewsRepository
{
    public class NewsReadRepository : ReadRepository<News>, INewsReadRepository
    {
        public NewsReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
