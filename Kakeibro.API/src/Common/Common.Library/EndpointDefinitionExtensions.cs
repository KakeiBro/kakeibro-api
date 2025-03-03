using System.Reflection;
using Common.Library.Interfaces;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Common.Library;

public static class EndpointDefinitionExtensions
{
    public static void InstallEndpointsFromAssemblies(
        this IEndpointRouteBuilder app, IConfiguration configuration, params Assembly[]? assemblies)
    {
        foreach (Assembly assembly in assemblies ?? [])
        {
            RegisterEndpoints(app, assembly);
        }
    }

    private static void RegisterEndpoints(IEndpointRouteBuilder app, Assembly assembly)
    {
        IEnumerable<IEndpointDefinition> endpointDefinitions = assembly
            .GetTypes()
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) &&
                        t is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach (IEndpointDefinition endpoint in endpointDefinitions)
        {
            endpoint.RegisterEndpoints(app);
        }
    }
}
