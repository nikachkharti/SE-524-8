using Microsoft.EntityFrameworkCore;
using MovieDB.Data;

namespace MovieDB
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var db = new MoviesContext();
        }
    }
}
