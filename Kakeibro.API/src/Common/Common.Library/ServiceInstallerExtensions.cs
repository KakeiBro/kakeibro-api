using System.Reflection;
using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Library;

public static class ServiceInstallerExtensions
{
    public static void InstallServicesFromAssemblies(
        this IServiceCollection app, IConfiguration configuration, params Assembly[]? assemblies)
    {
        foreach (Assembly assembly in assemblies ?? [])
        {
            IEnumerable<IServiceInstaller> endpointDefinitions = assembly
                .GetTypes()
                .Where(t => typeof(IServiceInstaller).IsAssignableFrom(t) &&
                            t is { IsInterface: false, IsAbstract: false })
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach (IServiceInstaller serviceInstaller in endpointDefinitions)
            {
                serviceInstaller.Install(app, configuration);
            }
        }
    }
}
