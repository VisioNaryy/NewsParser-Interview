namespace NewsParser.Infrastructure.Sql;

public class SqlSettings
{
    public const string SectionName = "SqlSettings";
    public Procedures Procedures { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class Procedures
{
    public string GetPostsByDateRange { get; set; }
    public string GetPostsWithSearchPhrase { get; set; }
    public string GetTopWords { get; set; }
    public string GetUserByEmail { get; set; }
    public string AddUser { get; set; }
}

public class ConnectionStrings
{
    public string AfterDbCreationConnection { get; set; }
    public string DefaultConnection { get; set; }
}