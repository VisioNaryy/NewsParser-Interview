namespace NewsParser.Application.HttpClients;

public interface ITechCrunchClient
{
    Task<string> GetHtmlContentFromPage(int pageNumber);
}