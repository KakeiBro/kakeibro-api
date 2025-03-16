using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Authentication.Data;

namespace Modules.Authentication.ServiceInstallers;

public class DataServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection app, IConfiguration configuration)
    {
        app.AddDatabase();
        app.AddHostedService<RedisMigrator>();
    }
}
