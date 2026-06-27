using Company524.API;
using Company524.API.Data;
using Company524.API.Models.Notification;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Company524.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // One InMemory database name per factory instance.
        // (Don't generate inside the AddDbContext lambda — it can run per scope
        //  and would split data across different in-memory stores.)
        private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";

        static CustomWebApplicationFactory()
        {
            // CRITICAL: Program.cs reads several config values (JWT:Secret, etc.)
            // BEFORE builder.Build() runs. WebApplicationFactory's
            // ConfigureAppConfiguration hooks run inside Build(), so those
            // in-memory values arrive TOO LATE for Program.cs to see them.
            //
            // Environment variables, on the other hand, are loaded by
            // WebApplication.CreateBuilder() at the very start — before any
            // Program.cs code runs. So we set them here in a static ctor
            // (runs once per test process, before any factory instance exists).
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", "InMemoryDb");

            Environment.SetEnvironmentVariable("JWT__Secret",
                "020F3137-3839-4459-9727-56B532FBB1CC-576BB07F-677F-480D-965E-5DF7074E9573");
            Environment.SetEnvironmentVariable("JWT__Issuer", "company524-api");
            Environment.SetEnvironmentVariable("JWT__Audience", "company524-client");
            Environment.SetEnvironmentVariable("JWT__AccessTokenExpiryMinutes", "15");
            Environment.SetEnvironmentVariable("JWT__RefreshTokenExpiryDays", "7");

            Environment.SetEnvironmentVariable("Jobs__RefreshTokenCleanupJob__ExpiryHours", "1");

            Environment.SetEnvironmentVariable("EmailSettings__Sender", "test@test.com");
            Environment.SetEnvironmentVariable("EmailSettings__SmtpServer", "localhost");
            Environment.SetEnvironmentVariable("EmailSettings__Port", "25");
            Environment.SetEnvironmentVariable("EmailSettings__UseSsl", "false");
            Environment.SetEnvironmentVariable("EmailSettings__Username", "test@test.com");
            Environment.SetEnvironmentVariable("EmailSettings__Password", "test");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // ----- Replace SQL Server DbContext with InMemory -----
                // AddDbContext registers multiple descriptors internally.
                // We need to remove ALL of them, otherwise EF Core sees both
                // SQL Server and InMemory providers registered and throws
                // "Services for database providers ... have been registered".
                var descriptorsToRemove = services
                    .Where(d =>
                        d.ServiceType == typeof(ApplicationDbContext) ||
                        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                        d.ServiceType == typeof(DbContextOptions) ||
                        (d.ServiceType.IsGenericType &&
                         d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextOptionsConfiguration<>) &&
                         d.ServiceType.GenericTypeArguments[0] == typeof(ApplicationDbContext)))
                    .ToList();

                foreach (var d in descriptorsToRemove)
                    services.Remove(d);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(_dbName));

                // ----- Replace ISmtpClient with a no-op mock -----
                services.RemoveAll<ISmtpClient>();
                var mockSmtp = new Mock<ISmtpClient>();
                mockSmtp.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                        .Returns(Task.CompletedTask);
                mockSmtp.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.CompletedTask);
                mockSmtp.Setup(s => s.SendAsync(It.IsAny<MimeKit.MimeMessage>()))
                        .Returns(Task.CompletedTask);
                mockSmtp.Setup(s => s.DisconnectAsync(It.IsAny<bool>()))
                        .Returns(Task.CompletedTask);
                services.AddSingleton(mockSmtp.Object);

                // ----- Replace IEmailService with a mock that always succeeds -----
                services.RemoveAll<IEmailService>();
                var mockEmail = new Mock<IEmailService>();
                mockEmail
                    .Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(new SendEmailResponse(true, "Email sent (mocked)"));
                services.AddSingleton(mockEmail.Object);

                // ----- Create the InMemory database schema -----
                // Use a scope on the ACTUAL service provider (Factory.Services)
                // so EnsureCreated runs against the same options the app uses.
                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
