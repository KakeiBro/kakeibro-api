using Common.Library.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Modules.Authentication.GoogleOAuth;

public class GoogleOAuthEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth", (IGoogleOAuthRepository repository) =>
        {
            repository.GenerateGoogleOAuthToken(string.Empty, string.Empty);
            return Results.Ok(new
            {
                Message = "Yes, I am Google Auth",
            });
        });
    }
}
