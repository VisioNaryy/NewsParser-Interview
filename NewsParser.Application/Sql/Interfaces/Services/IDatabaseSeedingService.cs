namespace NewsParser.Application.Sql.Interfaces.Services;

public interface IDatabaseSeedingService
{
    public Task SeedDatabase();
}