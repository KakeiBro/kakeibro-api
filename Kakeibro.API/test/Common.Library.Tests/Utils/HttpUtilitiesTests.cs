using System.Net;
using Common.Library.Utils;

namespace Common.Library.Tests.Utils;

public class HttpUtilitiesTests
{
    [Theory]
    [InlineData("GET", 200)]
    [InlineData("POST", 404)]
    [InlineData("PUT", 200)]
    [InlineData("DELETE", 200)]
    public async Task SendRequestWithQueryParamsAsync_WithVerbsAndResponse_ShouldWork(string method, int expectedResponse)
    {
        // Arrange
        var httpMethod = new HttpMethod(method);
        // If anything happens to jsonplaceholder, tests will break.
        const string uri = "https://jsonplaceholder.typicode.com/posts/1";

        // Act
        HttpResponseMessage result = await HttpUtilities.SendRequestWithQueryParamsAsync(
            uri,
            [
                new KeyValuePair<string, string>("a", "true"),
                new KeyValuePair<string, string>("b", "test"),
                new KeyValuePair<string, string>("c", "42"),
                new KeyValuePair<string, string>("d", "34.2"),
            ],
            httpMethod
        );
        string? queryParams = result.RequestMessage?.RequestUri?.Query;
        string? requestUri = result.RequestMessage?.RequestUri?.ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Equal((HttpStatusCode)expectedResponse, result.StatusCode);
        Assert.NotNull(requestUri);
        Assert.NotNull(queryParams);
        Assert.Contains(uri, requestUri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("?", queryParams, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(3, queryParams.Count(x => x == '&'));
        Assert.Contains("a=true", queryParams, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("b=test", queryParams, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("c=42", queryParams, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("d=34.2", queryParams, StringComparison.OrdinalIgnoreCase);
    }
}
