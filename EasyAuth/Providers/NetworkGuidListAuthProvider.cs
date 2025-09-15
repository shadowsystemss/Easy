using EasyAuth.Abstractions;

namespace EasyAuth.Providers;

public class NetworkGuidListAuthProvider(HttpClient httpClient, string url) : IGuidAuthorizeProvider
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _url = url;

    public async Task<bool> AuthorizeTestAsync(Guid UID, CancellationToken cancel)
    {
        try
        {
            string data = await _httpClient.GetStringAsync(_url, cancel);
            return data.Split('\n').Contains(UID.ToString());
        }
        catch
        {
            return false;
        }
    }
}
