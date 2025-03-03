using Common.Library;
using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Authentication;

public class AuthenticationModuleInstaller : IModuleInstaller
{
    public void InstallModule(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly);
    }
}
