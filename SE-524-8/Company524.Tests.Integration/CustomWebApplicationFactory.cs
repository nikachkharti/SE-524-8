using Company524.API;
using Company524.API.Data;
using Company524.API.Models.Notification;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Company524.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            // -------------------------------------------------------
            // Inject all configuration values the app needs at runtime.
            // This bypasses appsettings.json / appsettings.Development.json
            // entirely — no file-parsing errors, no missing values.
            // -------------------------------------------------------

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();

                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    // Connection string is irrelevant — we use InMemory DB —
                    // but Program.cs reads it before we swap the DbContext,
                    // so we provide a dummy value to avoid NullReferenceException.
                    ["ConnectionStrings:DefaultConnection"] = "InMemoryDb",

                    // JWT — must match exactly what JwtTokenGenerator uses
                    ["JWT:Secret"] = "020F3137-3839-4459-9727-56B532FBB1CC-576BB07F-677F-480D-965E-5DF7074E9573",
                    ["JWT:Issuer"] = "company524-api",
                    ["JWT:Audience"] = "company524-client",
                    ["JWT:AccessTokenExpiryMinutes"] = "15",
                    ["Jwt:RefreshTokenExpiryDays"] = "7",   // AuthService reads "Jwt:" (lowercase j)

                    // Background job config
                    ["Jobs:RefreshTokenCleanupJob:ExpiryHours"] = "1",

                    // Email settings — irrelevant (EmailService is mocked)
                    // but some DI registrations may read them on startup
                    ["EmailSettings:Sender"] = "test@test.com",
                    ["EmailSettings:SmtpServer"] = "localhost",
                    ["EmailSettings:Port"] = "25",
                    ["EmailSettings:UseSsl"] = "false",
                    ["EmailSettings:Username"] = "test@test.com",
                    ["EmailSettings:Password"] = "test"
                });
            });


            builder.ConfigureServices(services =>
            {
                // -------------------------------------------------------
                // STEP 1: Remove ALL DbContext-related registrations
                // -------------------------------------------------------
                // AddDbContext<T>() registers many descriptors internally.
                // We must remove all of them, not just DbContextOptions<T>,
                // otherwise SQL Server and InMemory both end up registered
                // → "two providers" error.

                var descriptorsToRemove = services
                    .Where(d =>
                        d.ServiceType == typeof(ApplicationDbContext) ||
                        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                        d.ServiceType == typeof(DbContextOptions) ||
                        (d.ImplementationFactory?.Target?.GetType().Name
                            .Contains("DbContext") ?? false))
                    .ToList();

                foreach (var d in descriptorsToRemove)
                    services.Remove(d);


                // Remove IDbContextOptionsConfiguration<ApplicationDbContext> descriptors
                var optionConfigs = services
                    .Where(d =>
                        d.ServiceType.IsGenericType &&
                        d.ServiceType.GetGenericTypeDefinition() ==
                            typeof(IDbContextOptionsConfiguration<>) &&
                        d.ServiceType.GenericTypeArguments[0] == typeof(ApplicationDbContext))
                    .ToList();

                foreach (var d in optionConfigs)
                    services.Remove(d);


                // -------------------------------------------------------
                // STEP 2: Register InMemory database
                // -------------------------------------------------------
                // Unique name per factory instance so parallel test runs
                // never share state.
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));


                // -------------------------------------------------------
                // STEP 3: Replace ISmtpClient with a Mock
                // -------------------------------------------------------
                var smtpDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISmtpClient));

                if (smtpDescriptor != null)
                    services.Remove(smtpDescriptor);

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


                // -------------------------------------------------------
                // STEP 4: Replace IEmailService with a Mock
                // -------------------------------------------------------
                var emailDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailService));

                if (emailDescriptor != null)
                    services.Remove(emailDescriptor);

                var mockEmail = new Mock<IEmailService>();
                mockEmail
                    .Setup(e => e.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                    .ReturnsAsync(new SendEmailResponse(true, "Email sent (mocked)"));
                services.AddSingleton(mockEmail.Object);



                // -------------------------------------------------------
                // STEP 5: Create InMemory database schema
                // -------------------------------------------------------
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // EnsureCreated() creates all tables from the entity model.
                // No migrations needed for InMemory.
                db.Database.EnsureCreated();

            });

        }
    }
}
