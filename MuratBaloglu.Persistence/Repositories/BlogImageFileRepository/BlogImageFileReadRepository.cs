using MuratBaloglu.Application.Repositories.BlogImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.BlogImageFileRepository
{
    public class BlogImageFileReadRepository : ReadRepository<BlogImageFile>, IBlogImageFileReadRepository
    {
        public BlogImageFileReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
