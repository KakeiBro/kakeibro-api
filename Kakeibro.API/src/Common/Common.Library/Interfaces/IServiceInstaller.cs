using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Library.Interfaces;

public interface IServiceInstaller
{
    void Install(IServiceCollection app, IConfiguration configuration);
}
