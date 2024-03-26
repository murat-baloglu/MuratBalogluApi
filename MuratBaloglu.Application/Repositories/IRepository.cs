using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Domain.Entities.Common;

namespace MuratBaloglu.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
