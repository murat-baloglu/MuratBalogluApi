using MuratBaloglu.Domain.Entities.Common;
using System.Linq.Expressions;

namespace MuratBaloglu.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> model, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> model, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);
    }
}
