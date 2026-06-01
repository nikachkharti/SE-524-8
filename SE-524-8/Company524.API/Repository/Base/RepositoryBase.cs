using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Company524.API.Repository.Base
{
    public class RepositoryBase<T, TContext> : IRepositoryBase<T, TContext>
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(TContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<T, object>> orderBy = null,
            bool ascending = true,
            CancellationToken cancellationToken = default,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            if (orderBy != null)
            {
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int page = Math.Max(pageNumber.Value, 1);
                int size = Math.Max(pageSize.Value, 1);

                query = query.Skip((page - 1) * size).Take(size);
            }

            var items = await query.ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            bool tracking = true,
            CancellationToken cancellationToken = default,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            // Supports ThenInclude chains
            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}
