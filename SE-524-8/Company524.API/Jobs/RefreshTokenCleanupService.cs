using Company524.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Company524.API.Jobs
{
    public class RefreshTokenCleanupService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<RefreshTokenCleanupService> logger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var jobExpireHours = configuration.GetValue<int>("Jobs:RefreshTokenCleanupJob:ExpiryHours");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var cutoff = DateTime.Now;

                    var deleted = await context.RefreshTokens
                        .Where(rt => rt.ExpiresAt < cutoff || rt.RevokedAt != null)
                        .ExecuteDeleteAsync(stoppingToken);

                    if (deleted > 0)
                        logger.LogInformation("Cleaned up {Count} expired/revoked refresh tokens", deleted);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during refresh token cleanup");
                }

                await Task.Delay(TimeSpan.FromHours(jobExpireHours), stoppingToken);
            }
        }
    }
}
