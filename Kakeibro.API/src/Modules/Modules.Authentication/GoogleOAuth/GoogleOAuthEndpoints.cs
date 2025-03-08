using Common.Library.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Modules.Authentication.Config;

namespace Modules.Authentication.GoogleOAuth;

public class GoogleOAuthEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth", (IGoogleOAuthRepository repository, IOptions<OAuthConfig> config) =>
        {
            repository.GenerateGoogleOAuthToken(string.Empty, string.Empty);
            return Results.Ok(config.Value);
        });
    }
}
