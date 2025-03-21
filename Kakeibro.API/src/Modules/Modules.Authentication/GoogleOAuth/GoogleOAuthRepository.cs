using Common.Library.Utils;
using Microsoft.Extensions.Options;
using Modules.Authentication.Config;
using QueryParams = Modules.Authentication.GoogleOAuth.Constants.OAuthQueryParams;
using QueryValues = Modules.Authentication.GoogleOAuth.Constants.OAuthQueryParamValues;

namespace Modules.Authentication.GoogleOAuth;

public interface IGoogleOAuthRepository
{
    Task<Uri> GenerateOAuthUriAsync(CancellationToken token);

    Task<string> GenerateGoogleOAuthTokenAsync(string code);
}

public class GoogleOAuthRepository(IOptions<OAuthConfig> config) : IGoogleOAuthRepository
{
    public async Task<Uri> GenerateOAuthUriAsync(CancellationToken token)
    {
        using var queryParameters = new FormUrlEncodedContent([
            new KeyValuePair<string, string>(QueryParams.ClientId, config.Value.ClientId!),
            new KeyValuePair<string, string>(QueryParams.RedirectUri, config.Value.RedirectUri!),
            new KeyValuePair<string, string>(QueryParams.ResponseType, QueryValues.ResponseType),
            new KeyValuePair<string, string>(QueryParams.Scope, QueryValues.Scope),
            new KeyValuePair<string, string>(QueryParams.AccessType, QueryValues.AccessType),
            new KeyValuePair<string, string>(QueryParams.Prompt, QueryValues.Prompt),
        ]);

        string queryString = await queryParameters.ReadAsStringAsync(token);
        var uriBuilder = new UriBuilder(config.Value.AuthorizationEndpoint!)
        {
            Query = queryString,
        };

        return uriBuilder.Uri;
    }

    public async Task<string> GenerateGoogleOAuthTokenAsync(string code)
    {
        HttpResponseMessage response = await HttpUtilities.SendRequestWithQueryParamsAsync(
            config.Value.TokenEndpoint!,
            [
                new KeyValuePair<string, string>(QueryParams.Code, code),
                new KeyValuePair<string, string>(QueryParams.ClientId, config.Value.ClientId!),
                new KeyValuePair<string, string>(QueryParams.ClientSecret, config.Value.ClientSecret!),
                new KeyValuePair<string, string>(QueryParams.RedirectUri, config.Value.RedirectUri!),
                new KeyValuePair<string, string>(QueryParams.GrantType, QueryValues.AuthorizationCode),
            ],
            HttpMethod.Post);

        return await response.Content.ReadAsStringAsync();
    }
}
