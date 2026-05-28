namespace Company524.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CONTAINER
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer("CONNECTION STRING")
            );

            builder.Services.AddControllers();

            var app = builder.Build();

            //PIPELINE
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
