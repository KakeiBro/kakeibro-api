using Common.Library.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Modules.Authentication.Config;

namespace Modules.Authentication.GoogleOAuth;

public record OAuthUriResponse(string Uri);

public class GoogleOAuthEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/auth-uri",
            async (IGoogleOAuthRepository repository, IOptions<OAuthConfig> config, CancellationToken token) =>
            {
                Uri response = await repository.GenerateOAuthUriAsync(token);

                return Results.Ok(new OAuthUriResponse(response.AbsoluteUri));
            });
    }
}
