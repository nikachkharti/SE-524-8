using Microsoft.EntityFrameworkCore;
using MovieDB.Data;
using MovieDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDB.Repositories
{
    public class GeneralRepository<T> where T : class
    {
        private readonly MoviesContext _context;
        public GeneralRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(int pageSize, int pageNumber)
        {
            return await _context.Set<T>()
                .Skip((pageNumber - 1) * pageSize) // OFFSET
                .Take(pageSize) //FETCH NEXT
                .ToListAsync();
        }
    }
}
