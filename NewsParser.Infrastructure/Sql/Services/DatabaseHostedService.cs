using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NewsParser.Infrastructure.Sql.Services;

public class DatabaseHostedService : IHostedService
{
    private readonly IServiceProvider _services;

    public DatabaseHostedService(IConfiguration configuration, IServiceProvider services)
    {
        _services = services;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CreateDatabase(cancellationToken);
        await SeedDatabase(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
    
    private async Task CreateDatabase(CancellationToken stoppingToken)
    {
        using (var scope = _services.CreateScope())
        {
            var databaseCreationService = 
                scope.ServiceProvider
                    .GetRequiredService<IDatabaseCreationService>();

            await databaseCreationService.CreateDatabase();
        }
    }

    private async Task SeedDatabase(CancellationToken token)
    {
        using (var scope = _services.CreateScope())
        {
            var databaseCreationService = 
                scope.ServiceProvider
                    .GetRequiredService<IDatabaseSeedingService>();

            await databaseCreationService.SeedDatabase();
        }
    }
}