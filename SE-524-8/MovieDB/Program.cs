using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MovieDB.Data;
using MovieDB.Entities;
using System.Diagnostics;

namespace MovieDB
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            using var db = new MoviesContext();

            await GetSingleFilm_NoCache(db);
            await GetSingleFilm_WithCache(db, cache);

        }

        static async Task GetSingleFilm_NoCache(MoviesContext db)
        {
            var sw = Stopwatch.StartNew();

            await db.Films
                .FirstOrDefaultAsync(f => f.FilmId == 1676);

            sw.Stop();

            Console.WriteLine($"Without cache: {sw.ElapsedMilliseconds} ms");
        }

        static async Task GetSingleFilm_WithCache(
            MoviesContext db,
            IMemoryCache cache)
        {
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1; i++)
            {
                await GetFilmCached(db, cache, 1676);
            }

            sw.Stop();

            Console.WriteLine($"With cache: {sw.ElapsedMilliseconds} ms");
        }

        static async Task<Film> GetFilmCached(MoviesContext db, IMemoryCache cache, int id)
        {
            var cacheKey = $"film-{id}";

            if (cache.TryGetValue(cacheKey, out Film film))
                return film;

            film = await db.Films
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FilmId == id);

            cache.Set(
                cacheKey,
                film,
                TimeSpan.FromMinutes(5));

            return film;
        }
    }
}
