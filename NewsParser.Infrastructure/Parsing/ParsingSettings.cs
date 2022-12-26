namespace NewsParser.Infrastructure.Parsing;

public class ParsingSettings
{
    public const string SectionName = "ParsingSettings";

    public Sections Sections { get; set; }

    public int MaxPageNumber { get; set; }

    public int MaxPostsNumber { get; set; }
}

public class Sections
{
    public string Start { get; set; }
    public string End { get; set; }
}