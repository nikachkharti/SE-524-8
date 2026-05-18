using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.Entities;

namespace MovieDB.Repositories
{
    public class CountryRepository
    {
        private readonly MoviesContext _context;

        public CountryRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetAllCountriesAsync(int pageSize, int pageNumber)
        {
            return await _context.Countries
                .Skip((pageNumber - 1) * pageSize) // OFFSET
                .Take(pageSize) //FETCH NEXT
                .ToListAsync();
        }
    }
}
