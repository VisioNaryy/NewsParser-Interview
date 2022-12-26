using Microsoft.Extensions.Configuration;
using NewsParser.Contracts.Parsing;

namespace NewsParser.Infrastructure.Sql.Services;

public class DatabaseSeedingService : IDatabaseSeedingService
{
    private readonly SqlSettings _sqlSettings;
    private readonly ParsingSettings _parsingSettings;
    private readonly IParsingService _parsingService;

    public DatabaseSeedingService(IOptions<SqlSettings> sqlSettings, IOptions<ParsingSettings> parsingSettings, IConfiguration configuration, IParsingService parsingService)
    {
        _sqlSettings = sqlSettings.Value;
        _parsingSettings = parsingSettings.Value;
        _parsingService = parsingService;
    }

    public async Task SeedDatabase()
    {
        var posts = new List<Post>();

        await foreach (var post in _parsingService.GetPosts())
        {
            posts.Add(post);
            
            if (posts.Count == _parsingSettings.MaxPostsNumber)
                break;
        }

        await FillPostsTable(posts);
    }

    private async Task FillPostsTable(List<Post> posts)
    {
        using (var connection = new SqlConnection(_sqlSettings.ConnectionStrings.AfterDbCreationConnection))
        {
            await connection.OpenAsync();

            var command = new SqlCommand("USE [NewsParserProductionDb] ", connection);
            await command.ExecuteNonQueryAsync();

            foreach (var post in posts)
            {
                command.CommandText = $@"INSERT INTO [dbo].[Posts] (Link, Title, Content, DateGmt) values ('{post.Link}','{post.Title?.Rendered}','{post.Content?.Rendered}','{post.Date}')";
                await command.ExecuteNonQueryAsync();
            }
        };
    }
}