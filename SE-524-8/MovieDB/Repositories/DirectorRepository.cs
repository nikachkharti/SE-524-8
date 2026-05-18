using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.Entities;

namespace MovieDB.Repositories
{
    public class DirectorRepository
    {
        private readonly MoviesContext _context;

        public DirectorRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<List<Director>> GetAllDirectorsAsync(int pageSize, int pageNumber)
        {
            return await _context.Directors
                .Skip((pageNumber - 1) * pageSize) // OFFSET
                .Take(pageSize) //FETCH NEXT
                .ToListAsync();
        }

    }
}
