using Microsoft.Extensions.Options;
using Modules.Authentication.Config;

namespace Modules.Authentication.GoogleOAuth;

public interface IGoogleOAuthRepository
{
    Task<Uri> GenerateOAuthUriAsync(CancellationToken token);

    void GenerateGoogleOAuthToken(string clientId, string clientSecret);
}

public class GoogleOAuthRepository(IOptions<OAuthConfig> config) : IGoogleOAuthRepository
{
    public async Task<Uri> GenerateOAuthUriAsync(CancellationToken token)
    {
        using var queryParameters = new FormUrlEncodedContent([
            new KeyValuePair<string, string>(Constants.OAuthQueryParams.ClientId, config.Value.ClientId!),
            new KeyValuePair<string, string>(Constants.OAuthQueryParams.RedirectUri, config.Value.RedirectUri!),
            new KeyValuePair<string, string>(Constants.OAuthQueryParams.ResponseType, Constants.OAuthQueryParamValues.ResponseType),
            new KeyValuePair<string, string>(Constants.OAuthQueryParams.Scope, Constants.OAuthQueryParamValues.Scope),
            new KeyValuePair<string, string>(Constants.OAuthQueryParams.AccessType, Constants.OAuthQueryParamValues.AccessType),
            new KeyValuePair<string, string>(Constants.OAuthQueryParams.Prompt, Constants.OAuthQueryParamValues.Prompt),
        ]);

        string queryString = await queryParameters.ReadAsStringAsync(token);
        var uriBuilder = new UriBuilder(config.Value.AuthorizationEndpoint!)
        {
            Query = queryString,
        };

        return uriBuilder.Uri;
    }

    public void GenerateGoogleOAuthToken(string clientId, string clientSecret)
    {
        Console.WriteLine("Generating Google OAuth Token...");
    }
}
