using Company524.API.Data;
using Company524.API.Repository;
using Company524.API.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Company524.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //CONTROLLERS
            builder.Services.AddControllers();

            //SWAGGER
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SCAA API",
                    Version = "v1",
                    Description = "API for SCAA final project"
                });


                #region XML Documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                #endregion


                #region Examples
                options.ExampleFilters();
                #endregion
            });

            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();


            //DATABASE
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            //var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


            //REPOSITORIES
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();


            var app = builder.Build();

            //DB AUTO UPDATE
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();


            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
