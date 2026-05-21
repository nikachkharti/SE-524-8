using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using System.Linq.Expressions;

namespace MovieDB.Repositories
{
    public class GeneralRepository<T> where T : class
    {
        private readonly MoviesContext _context;
        public GeneralRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            int pageNumber,
            int pageSize,
            bool ascending = true,
            Expression<Func<T, object>> orderBy = null,
            Expression<Func<T, bool>> filter = null,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
            {
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<T> GetAsync(
            Expression<Func<T, bool>> filter,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(filter);
        }


        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Remove(T entity) => _context.Set<T>().Remove(entity);
        public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AnyAsync(predicate);
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

    }
}
