using Microsoft.Extensions.Options;
using Modules.Authentication.Config;
using Modules.Authentication.GoogleOAuth;

namespace Modules.Authentication.Tests.GoogleOAuth;

public class GoogleOAuthRepositoryTests
{
    private readonly OptionsWrapper<OAuthConfig> _config;

    public GoogleOAuthRepositoryTests()
    {
        _config = new OptionsWrapper<OAuthConfig>(new OAuthConfig
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            RedirectUri = "test-redirect-uri",
            AuthorizationEndpoint = "https://www.test-google/oauth",
        });
    }

    [Fact]
    public async Task GenerateOAuthUriAsync_WithInjectedConfiguration_GeneratesUriCorrectly()
    {
        // Arrange
        var service = new GoogleOAuthRepository(_config);

        // Act
        Uri result = await service.GenerateOAuthUriAsync(CancellationToken.None);
        string uri = result.AbsoluteUri;

        // Assert
        Assert.NotNull(result);
        Assert.Contains(_config.Value.AuthorizationEndpoint!, uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains($"client_id={_config.Value.ClientId}", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains($"redirect_uri={_config.Value.RedirectUri}", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("response_type=code", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("scope=openid+email+profile", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("access_type=offline", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("prompt=consent", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("?", uri, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(5, uri.Count(x => x == '&'));
    }
}
