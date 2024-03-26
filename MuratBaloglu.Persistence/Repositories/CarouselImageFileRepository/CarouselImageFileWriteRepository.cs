using MuratBaloglu.Application.Repositories.CarouselImageFileRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.CarouselImageFileRepository
{
    public class CarouselImageFileWriteRepository : WriteRepository<CarouselImageFile>, ICarouselImageFileWriteRepository
    {
        public CarouselImageFileWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
