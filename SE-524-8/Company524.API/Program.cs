using Company524.API.Data;
using Company524.API.Mapping;
using Company524.API.Middleware;
using Company524.API.Models.Authentication;
using Company524.API.Repository;
using Company524.API.Repository.Contracts;
using Company524.API.Service;
using Company524.API.Service.Contracts;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                    Title = "Company524",
                    Version = "v1",
                    Description = "API for education"
                });


                #region Security Header
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Just paste your token below (without the 'Bearer ' prefix)"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
                #endregion


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
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            //REPOSITORIES
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();


            //SERVICES
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


            //MAPSTER
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MappingConfig).Assembly);

            builder.Services.AddSingleton(config);
            builder.Services.AddScoped<IMapper, ServiceMapper>();


            //IDENTITY
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 9;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            //AUTHENTICATION
            var secret = builder.Configuration.GetValue<string>("JWT:Secret");
            var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
            var audience = builder.Configuration.GetValue<string>("JWT:Audience");
            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = issuer,
                    ValidAudience = audience
                };
            });



            var app = builder.Build();


            //DB AUTO UPDATE
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed: {ex.Message}", ex);
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
