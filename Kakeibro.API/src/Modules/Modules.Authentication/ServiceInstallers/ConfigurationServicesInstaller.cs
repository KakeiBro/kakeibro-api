using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Authentication.Config;

namespace Modules.Authentication.ServiceInstallers;

public class ConfigurationServicesInstaller : IServiceInstaller
{
    private const string OAuthConfigSectionName = "GoogleAuth";

    public void Install(IServiceCollection app, IConfiguration configuration)
    {
        app.Configure<OAuthConfig>(configuration.GetSection(OAuthConfigSectionName));
    }
}
