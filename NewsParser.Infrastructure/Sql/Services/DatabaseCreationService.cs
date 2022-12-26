using System.Reflection;
using System.Text.RegularExpressions;

namespace NewsParser.Infrastructure.Sql.Services;

public class DatabaseCreationService: IDatabaseCreationService
{
    private readonly SqlSettings _sqlSettings;

    public DatabaseCreationService(IOptions<SqlSettings> sqlSettings)
    {
        _sqlSettings = sqlSettings.Value;
    }

    public async Task CreateDatabase()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var proceduresPath = Path.Combine(path, "Sql", "CreationQueries", "CreateAllProcedures.sql");
        var dbPath = Path.Combine(path, "Sql", "CreationQueries", "CreateDbWithTables.sql");
        
        var createAllProcedures = await File.ReadAllTextAsync(proceduresPath);
        var createDbWithTables = await File.ReadAllTextAsync(dbPath);
        
        using (var connection = new SqlConnection(_sqlSettings.ConnectionStrings.DefaultConnection))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("", connection);
            
            var scripts = Regex.Split(createDbWithTables, @"(\s+|;|\n|\r)GO", RegexOptions.Multiline);
            foreach(var splitScript in scripts.Where(splitScript => !string.IsNullOrWhiteSpace(splitScript))) {
                command.CommandText = splitScript;
                await command.ExecuteNonQueryAsync();
            }
        };
        
        using (var connection = new SqlConnection(_sqlSettings.ConnectionStrings.DefaultConnection))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("", connection);
            
            var scripts = Regex.Split(createAllProcedures, @"(\s+|;|\n|\r)GO", RegexOptions.Multiline);
            foreach(var splitScript in scripts.Where(splitScript => !string.IsNullOrWhiteSpace(splitScript))) {
                command.CommandText = splitScript;
                await command.ExecuteNonQueryAsync();
            }
        };

    }
}