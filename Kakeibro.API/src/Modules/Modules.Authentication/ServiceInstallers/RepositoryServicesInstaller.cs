using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Authentication.GoogleOAuth;

namespace Modules.Authentication.ServiceInstallers;

public class RepositoryServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection app, IConfiguration configuration)
    {
        app.AddScoped<IGoogleOAuthRepository, GoogleOAuthRepository>();
    }
}
