using MuratBaloglu.Application.Repositories.BlogImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.BlogImageFileRepository
{
    public class BlogImageFileWriteRepository : WriteRepository<BlogImageFile>, IBlogImageFileWriteRepository
    {
        public BlogImageFileWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
