using MuratBaloglu.Application.Repositories.VideoRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.VideoRepository
{
    public class VideoReadRepository : ReadRepository<Video>, IVideoReadRepository
    {
        public VideoReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
