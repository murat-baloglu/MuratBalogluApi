using MuratBaloglu.Application.Repositories.NewsImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.NewsImageFileRepository
{
    public class NewsImageFileReadRepository : ReadRepository<NewsImageFile>, INewsImageFileReadRepository
    {
        public NewsImageFileReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
