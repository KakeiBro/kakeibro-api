using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Authentication.Config;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;

namespace Modules.Authentication.Data;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services)
    {
        IOptions<RedisConfig> config = services.BuildServiceProvider().GetService<IOptions<RedisConfig>>()!;

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{config.Value.EndpointUri}:{config.Value.Port}" },
                Password = string.Empty,
            };
        });
        services.AddSingleton<IRedisConnectionProvider>(
            new RedisConnectionProvider($"{config.Value.EndpointConnectionString}:{config.Value.Port}"));

        return services;
    }
}
