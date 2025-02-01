namespace DeepSeekOllamaAspire.Web;

public class ChatApiClient(HttpClient httpClient)
{
    public async Task<string> ChatAsync(string question, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/chat?question={question}";
        var response = await httpClient.GetFromJsonAsync<Response>(requestUri, cancellationToken);
        return response?.Value ?? "";
    }
}

public record Response(string Value);
