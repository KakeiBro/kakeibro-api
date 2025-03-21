namespace Common.Library.Utils;

public static class HttpUtilities
{
    public static async Task<HttpResponseMessage> SendRequestWithQueryParamsAsync(
        string uri, IEnumerable<KeyValuePair<string, string>> queryParams, HttpMethod method)
    {
        using var requestContent = new FormUrlEncodedContent(queryParams);
        var uriBuilder = new UriBuilder(uri)
        {
            Query = await requestContent.ReadAsStringAsync()
        };
        using var request = new HttpRequestMessage(method, uriBuilder.Uri);
        using var httpClient = new HttpClient();

        HttpResponseMessage response = await httpClient.SendAsync(request);

        return response;
    }
}
