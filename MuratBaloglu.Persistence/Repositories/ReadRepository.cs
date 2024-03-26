using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Application.Repositories;
using MuratBaloglu.Domain.Entities.Common;
using MuratBaloglu.Persistence.Contexts;
using System.Linq.Expressions;

namespace MuratBaloglu.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly MuratBalogluDbContext _context;

        public ReadRepository(MuratBalogluDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = Table.Where(method).AsNoTracking();
            return query;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);

        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            //return await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            //await Table.FindAsync(Guid.Parse(id));
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }
    }
}
