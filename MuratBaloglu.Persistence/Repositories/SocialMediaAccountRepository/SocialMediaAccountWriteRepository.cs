using MuratBaloglu.Application.Repositories.SocialMediaAccountRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SocialMediaAccountRepository
{
    public class SocialMediaAccountWriteRepository : WriteRepository<SocialMediaAccount>, ISocialMediaAccountWriteRepository
    {
        public SocialMediaAccountWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
