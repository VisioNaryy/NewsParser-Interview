using NewsParser.Contracts.Parsing;

namespace NewsParser.Application.Parsing.Interfaces.Services;

public interface IParsingService
{
    IAsyncEnumerable<Post> GetPosts();
}