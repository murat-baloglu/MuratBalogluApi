using MuratBaloglu.Application.Repositories.BlogRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.BlogRepository
{
    public class BlogWriteRepository : WriteRepository<Blog>, IBlogWriteRepository
    {
        public BlogWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
