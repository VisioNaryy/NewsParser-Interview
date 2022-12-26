using NewsParser.Domain.Common.Models;
using NewsParser.Contracts.Posts;
namespace NewsParser.Application.Common.Interfaces.Repositories;

public interface IPostsRepository
{
    Task<IEnumerable<Post>> GetPostsByParamsAsync(PostParams postParams);
    Task<IEnumerable<string>> GetTopWordsAsync(int? number);
    Task<IEnumerable<Post>> GetPostsBySearchPhraseAsync(string searchPhrase);
}