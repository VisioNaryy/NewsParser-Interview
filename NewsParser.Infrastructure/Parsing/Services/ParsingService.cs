using System.Text.RegularExpressions;
using NewsParser.Contracts.Parsing;

namespace NewsParser.Infrastructure.Parsing.Services;

public class ParsingService : IParsingService
{
    private readonly ParsingSettings _parsingSettings;
    private readonly ITechCrunchClient _techCrunchClient;
    private readonly ICustomJsonSerializer<Entity> _customJsonSerializer;

    public ParsingService(IOptions<ParsingSettings> parsingSettings, ITechCrunchClient techCrunchClient,
        ICustomJsonSerializer<Entity> customJsonSerializer)
    {
        _parsingSettings = parsingSettings.Value;
        _techCrunchClient = techCrunchClient;
        _customJsonSerializer = customJsonSerializer;
    }

    public async IAsyncEnumerable<Post> GetPosts()
    {
        var sectionStart = _parsingSettings.Sections.Start;
        var sectionEnd = _parsingSettings.Sections.End;
        int pageNumber = 1;

        do
        {
            var html = await _techCrunchClient.GetHtmlContentFromPage(pageNumber);

            var entityString = FindSection(html, sectionStart, sectionEnd);

            var entity = _customJsonSerializer.Deserialize(entityString);

            foreach (var post in entity?.Posts)
            {
                var content = post.Content;
                var title = post.Title;

                if (!string.IsNullOrWhiteSpace(content?.Rendered))
                    content.Rendered = StripHtmlAndEscapeUnicode(content.Rendered);

                if (!string.IsNullOrWhiteSpace(title?.Rendered))
                    title.Rendered = StripHtmlAndEscapeUnicode(title.Rendered);

                yield return post;
            }

            pageNumber++;
        } while (pageNumber <= _parsingSettings.MaxPageNumber);
    }

    private string StripHtmlAndEscapeUnicode(string input)
        =>
            Regex.Replace(input, @"<[^>]*>|\\u\d+|&#\d+;|'", string.Empty);

    private string FindSection(string html, string sectionStart, string sectionEnd)
    {
        var htmlSpan = html.AsSpan();

        var startIndex = htmlSpan.IndexOf(sectionStart.AsSpan());
        var endIndex = htmlSpan.IndexOf(sectionEnd.AsSpan()) - startIndex;

        var sliced = htmlSpan.Slice(startIndex, endIndex);

        return '{' + $"{sliced}" + '}';
    }
}