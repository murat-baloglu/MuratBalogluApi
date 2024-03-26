using MuratBaloglu.Application.Repositories.BlogRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.BlogRepository
{
    public class BlogReadRepository : ReadRepository<Blog>, IBlogReadRepository
    {
        public BlogReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
