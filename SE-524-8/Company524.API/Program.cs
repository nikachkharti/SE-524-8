using CloudinaryDotNet;
using Company524.API.Jobs;
using Company524.API.Middleware;
using Company524.Application.Contracts.Persistence;
using Company524.Application.Contracts.Service;
using Company524.Application.Mapping;
using Company524.Application.Models.Cloudinary;
using Company524.Application.Service;
using Company524.Domain.Entities;
using Company524.Infrastructure.Data;
using Company524.Infrastructure.ExternalServices.DummyJson;
using Company524.Infrastructure.Persistence;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
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
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();


            //SERVICES
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddSingleton<ISmtpClient, SmtpClientWrapper>();
            builder.Services.AddSingleton<IEmailService, EmailService>();

            //HTTP CLIENTS
            builder.Services.AddHttpClient<IExternalProductCatalogService, DummyJsonProductCatalogService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ExternalApis:DummyJson:BaseUrl"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            });


            //BACKGROUND SERVICES (JOBS)
            //builder.Services.AddHostedService<RefreshTokenCleanupService>();


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



            //CLOUDINARY
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

            var cloudinarySettings = builder.Configuration
                .GetSection("Cloudinary")
                .Get<CloudinarySettings>();

            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );

            var cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };

            builder.Services.AddSingleton(cloudinary);
            builder.Services.AddScoped<ICloudinaryImageService, CloudinaryImageService>();



            var app = builder.Build();


            //DB AUTO UPDATE. skip for tesing environment (integration tests use InMemory DB)
            if (!builder.Environment.IsEnvironment("Testing"))
            {
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
