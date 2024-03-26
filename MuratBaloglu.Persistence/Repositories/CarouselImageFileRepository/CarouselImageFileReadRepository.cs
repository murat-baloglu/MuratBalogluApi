using MuratBaloglu.Application.Repositories.CarouselImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.CarouselImageFileRepository
{
    public class CarouselImageFileReadRepository : ReadRepository<CarouselImageFile>, ICarouselImageFileReadRepository
    {
        public CarouselImageFileReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
