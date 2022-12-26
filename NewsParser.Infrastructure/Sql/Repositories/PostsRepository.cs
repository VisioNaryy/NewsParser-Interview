using NewsParser.Application.Parsing.Errors;
using NewsParser.Contracts.Posts;
using NewsParser.Infrastructure.Extensions;
using NewsParser.Domain.Common.Models;

namespace NewsParser.Infrastructure.Sql.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly SqlSettings _sqlSettings;
    private readonly SqlServerDatabaseContext _context;

    public PostsRepository(IOptions<SqlSettings> sqlSettings, SqlServerDatabaseContext context)
    {
        _sqlSettings = sqlSettings.Value;
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetPostsByParamsAsync(PostParams postParams)
    {
        var (from, to) = postParams;

        if (!DateTime.TryParse(from, out var fromParsed) || !DateTime.TryParse(to, out var toParsed))
            throw new IncorrectDataException();

        var startDateParameter = new SqlParameter("@start_date", fromParsed);
        var endDateParameter = new SqlParameter("@end_date", toParsed);

        var procedureName = _sqlSettings.Procedures.GetPostsByDateRange;

        _context.CreateProcedureWithParameters(procedureName, new[] {startDateParameter, endDateParameter});

        return await BuildEntityList(await _context.CreateDataReaderAsync());
    }

    public async Task<IEnumerable<string>> GetTopWordsAsync(int? number)
    {
        var numberParameter = _context.CreateParameter("@top_count", number ?? 10);

        var procedureName = _sqlSettings.Procedures.GetTopWords;

        _context.CreateProcedureWithParameters(procedureName, new[] {numberParameter});

        return await BuildCollection(await _context.CreateDataReaderAsync());
    }

    public async Task<IEnumerable<Post>> GetPostsBySearchPhraseAsync(string searchPhrase)
    {
        var searchPhraseParameter = _context.CreateParameter("@search_phrase", searchPhrase);

        var procedureName = _sqlSettings.Procedures.GetPostsWithSearchPhrase;

        _context.CreateProcedureWithParameters(procedureName, new[] {searchPhraseParameter});

        return await BuildEntityList(await _context.CreateDataReaderAsync());
    }

    private async Task<IEnumerable<Post>> BuildEntityList(SqlDataReader rdr)
    {
        List<Post> posts = new();

        while (await rdr.ReadAsync())
        {
            posts.Add(new Post(
                rdr.GetData<string>("Link"),
                rdr.GetData<string>("Title"),
                rdr.GetData<string>("Content"),
                rdr.GetData<DateTime?>("DateGmt")?.ToString()
            ));
        }

        return posts;
    }

    private async Task<IEnumerable<string>> BuildCollection(SqlDataReader rdr)
    {
        var collection = new List<string>();

        while (await rdr.ReadAsync())
            collection.Add(rdr.GetString("word"));

        return collection;
    }
}