using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Library.Interfaces;

public interface IModuleInstaller
{
    void InstallModule(IServiceCollection serviceCollection, IConfiguration configuration);
}
