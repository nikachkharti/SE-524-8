using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Company524.API.Repository.Base
{
    public interface IRepositoryBase<T, TContext>
            where T : class
            where TContext : DbContext
    {
        Task<(IEnumerable<T> Items, int TotalCount)> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<T, object>> orderBy = null,
            bool ascending = true,
            CancellationToken cancellationToken = default,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes);
        Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            bool tracking = true,
            CancellationToken cancellationToken = default,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task AddAsync(T entity);
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
        void Remove(T entity);
        void Update(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
