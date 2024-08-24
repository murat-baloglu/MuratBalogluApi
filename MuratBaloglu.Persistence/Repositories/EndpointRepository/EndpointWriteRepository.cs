using MuratBaloglu.Application.Repositories.EndpointRepository;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Persistence.Contexts;

namespace MuratBaloglu.Persistence.Repositories.EndpointRepository
{
    public class EndpointWriteRepository : WriteRepository<Endpoint>, IEndpointWriteRepository
    {
        public EndpointWriteRepository(MuratBalogluDbContext context) : base(context)
        {
        }
    }
}
