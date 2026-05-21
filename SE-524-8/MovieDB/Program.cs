using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using MovieDB.Data;
using MovieDB.Entities;
using MovieDB.Repositories;
using System.Diagnostics;

namespace MovieDB
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            #region ქეშირება
            //var cache = new MemoryCache(new MemoryCacheOptions());
            //using var db = new MoviesContext();

            //await GetSingleFilm_NoCache(db);
            //await GetSingleFilm_WithCache(db, cache); 
            #endregion

            #region პროცედურის გამოძახება EF Core _ ით
            //using var db = new MoviesContext();

            //const string sqlQuery = "EXEC dbo.sp_GetPagedFilms @PageNumber, @PageSize";
            //var pageNumberParameter = new SqlParameter("@PageNumber", 2);
            //var pageSizeParameter = new SqlParameter("@PageSize", 10);


            //var filteredMovieBenchmarks = await db.FilmsBenchmark
            //    .FromSqlRaw(sqlQuery, pageNumberParameter, pageSizeParameter)
            //    .ToListAsync(); 
            #endregion



            using var db = new MoviesContext();

            GeneralRepository<Film> repository = new(db);

            var result = await repository.GetAllAsync(
                pageNumber: 1,
                pageSize: 5,
                ascending: false,
                orderBy: x => x.ReleaseDate,
                filter: x => x.ReleaseDate != null,
                includes: [x => x.Director, x => x.Studio, x => x.Country, x => x.Language, x => x.Genre, x => x.Certificate]
            );


        }

        //შექმენით რეპოზიოტრის კლასი უნდა შემეძლოს paging

        //1. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა მსახიობს EF CORE CODE FIRST + LINQ
        //2. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა რეჟისორს EF CORE CODE FIRST + LINQ
        //3. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა სერთიფიკატს EF CORE CODE FIRST + LINQ
        //4. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა ქვეყანას EF CORE CODE FIRST + LINQ
        //5. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა ჟანრს EF CORE CODE FIRST + LINQ
        //6. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა ენას EF CORE CODE FIRST + LINQ
        //7. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა როლს EF CORE CODE FIRST + LINQ
        //8. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა ფილმს EF CORE CODE FIRST + LINQ
        //8. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა სტუდიას EF CORE CODE FIRST + LINQ
        //9. დაწერეთ მეთოდი რომელიც წამოიღებს ყველა სტუდიას EF CORE CODE FIRST + LINQ




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
