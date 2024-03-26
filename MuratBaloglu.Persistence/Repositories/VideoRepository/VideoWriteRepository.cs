using MuratBaloglu.Application.Repositories.VideoRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.VideoRepository
{
    public class VideoWriteRepository : WriteRepository<Video>, IVideoWriteRepository
    {
        public VideoWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
