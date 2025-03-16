using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Authentication.Config;
using Modules.Authentication.Data;

namespace Modules.Authentication.ServiceInstallers;

public class DataServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection app, IConfiguration configuration)
    {
        IOptions<RedisConfig> config = app.BuildServiceProvider().GetService<IOptions<RedisConfig>>()!;
        if (config.Value.RedisOff)
        {
            return;
        }

        app.AddDatabase();
        app.AddHostedService<RedisMigrator>();
    }
}
