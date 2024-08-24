using MuratBaloglu.Application.Repositories.EndpointRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.EndpointRepository
{
    public class EndpointReadRepository : ReadRepository<Endpoint>, IEndpointReadRepository
    {
        public EndpointReadRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
