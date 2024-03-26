using MuratBaloglu.Application.Repositories.SocialMediaAccountRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.SocialMediaAccountRepository
{
    public class SocialMediaAccountReadRepository : ReadRepository<SocialMediaAccount>, ISocialMediaAccountReadRepository
    {
        public SocialMediaAccountReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
