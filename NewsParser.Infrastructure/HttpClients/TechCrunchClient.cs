namespace NewsParser.Infrastructure.HttpClients;

public class TechCrunchClient : ITechCrunchClient
{
    private readonly HttpClient _httpClient;

    public TechCrunchClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> GetHtmlContentFromPage(int pageNumber)
    {
        var httpResponse = await _httpClient.GetAsync($"page/{pageNumber}/", HttpCompletionOption.ResponseHeadersRead);

        httpResponse.EnsureSuccessStatusCode();
        
        return await httpResponse.Content.ReadAsStringAsync();
    }
}