using MuratBaloglu.Application.Repositories.NewsImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.NewsImageFileRepository
{
    public class NewsImageFileWriteRepository : WriteRepository<NewsImageFile>, INewsImageFileWriteRepository
    {
        public NewsImageFileWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
