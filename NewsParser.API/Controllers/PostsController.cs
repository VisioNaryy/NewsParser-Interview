using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.Application.Common.Interfaces.Repositories;
using NewsParser.Contracts.Posts;
using NewsParser.Domain.Common.Models;

namespace NewsParser.API.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostsRepository _postsRepository;

    public PostsController(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IEnumerable<Post>> GetPostsBySearchPhrase([FromQuery(Name = "text")] string searchPhrase)
        => 
            await _postsRepository.GetPostsBySearchPhraseAsync(searchPhrase);

    [HttpGet]
    [Route("posts")]
    public async Task<IEnumerable<Post>> GetPostsByParams([FromQuery] PostParams parameters)
        => 
            await _postsRepository.GetPostsByParamsAsync(parameters);

    [HttpGet]
    [Route("topten")]
    public async Task<IEnumerable<string>> GetTopWords([FromQuery] int? number)
        => 
            await _postsRepository.GetTopWordsAsync(number);
}