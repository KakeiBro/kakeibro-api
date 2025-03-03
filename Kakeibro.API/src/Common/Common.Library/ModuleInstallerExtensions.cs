using System.Reflection;
using Common.Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Library;

public static class ModuleInstallerExtensions
{
    public static void InstallModulesFromAssemblies(
        this IServiceCollection app,
        IConfiguration configuration,
        params Assembly[]? assemblies)
    {
        foreach (Assembly assembly in assemblies ?? [])
        {
            InstallModules(app, configuration, assembly);
        }
    }

    private static void InstallModules(
        IServiceCollection app, IConfiguration configuration, Assembly assembly)
    {
        IEnumerable<IModuleInstaller> endpointDefinitions = assembly
            .GetTypes()
            .Where(t => typeof(IModuleInstaller).IsAssignableFrom(t) &&
                        t is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IModuleInstaller>();

        foreach (IModuleInstaller moduleInstaller in endpointDefinitions)
        {
            moduleInstaller.InstallModule(app, configuration);
        }
    }
}
