using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modules.Authentication.Config;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Modules.Authentication.Data;

public interface IRedisMigrator
{
    Task InitializeMigrationsAsync();

    Task MigrateAsync();

    Task SeedAsync();
}

public class RedisMigrator(
    IOptions<RedisConfig> config,
    ILogger<RedisMigrator> logger,
    IDistributedCache cache,
    IRedisConnectionProvider provider)
    : IRedisMigrator, IHostedService
{
    private const string MigrationsKey = "SchemaMigration:Version";
    private const string LatestMigrationNumber = "20250316";
    private bool _isUpToDate;

    public async Task InitializeMigrationsAsync()
    {
        string? appliedMigration = await cache.GetStringAsync(MigrationsKey);

        _isUpToDate = (appliedMigration ?? string.Empty)
            .Equals(LatestMigrationNumber, StringComparison.OrdinalIgnoreCase);

        if (!config.Value.ForceMigration && _isUpToDate)
        {
            return;
        }

        await cache.SetStringAsync(MigrationsKey, LatestMigrationNumber);
    }

    public async Task MigrateAsync()
    {
        if (!config.Value.ForceMigration && _isUpToDate)
        {
            return;
        }

        IEnumerable<Task> indexTasks = Constants.Indexes
            .Select(index => provider.Connection.CreateIndexAsync(index));

        await Task.WhenAll(indexTasks);
    }

    public async Task SeedAsync()
    {
        if (!config.Value.Seed)
        {
            return;
        }

        Faker<Token>? tokens = new Faker<Token>()
            .StrictMode(true)
            .RuleFor(x => x.TokenId, (f, _) => f.IndexFaker + 1)
            .RuleFor(x => x.Value, (f, _) => f.System.ApplePushToken())
            .RuleFor(x => x.Vendor, (f, _) => f.Company.CompanyName())
            .RuleFor(x => x.Deleted, _ => false);

        Faker<MfaDevice>? mfaDevices = new Faker<MfaDevice>()
            .StrictMode(true)
            .RuleFor(x => x.MfaDeviceId, (f, _) => f.IndexFaker + 1)
            .RuleFor(x => x.Name, (f, _) => f.Company.CompanyName())
            .RuleFor(x => x.Deleted, _ => false);

        Faker<User>? userFixture = new Faker<User>()
            .StrictMode(true)
            .RuleFor(x => x.UserId, (f, _) => f.IndexFaker + 1)
            .RuleFor(x => x.FirstName, (f, _) => f.Name.FirstName())
            .RuleFor(x => x.LastName, (f, _) => f.Name.LastName())
            .RuleFor(x => x.Email, (f, _) => f.Internet.Email())
            .RuleFor(x => x.Deleted, (_, _) => false)
            .RuleFor(x => x.Tokens, _ => tokens.Generate(3))
            .RuleFor(x => x.MfaDevices, _ => mfaDevices.Generate(3));

        IRedisCollection<User> users = provider.RedisCollection<User>();

        List<User> seedValues = userFixture.Generate(10);

        await users.InsertAsync(seedValues);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting redis migrator");
        await InitializeMigrationsAsync();
        await MigrateAsync();
        await SeedAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping redis migrator");
        if (!config.Value.CleanOnShutdown)
        {
            return;
        }

        Task<bool>[] indexTasks = Constants.Indexes
            .Select(index =>
                Task.Run(
                    () => provider.Connection.DropIndexAndAssociatedRecords(index),
                    cancellationToken))
            .ToArray();

        await Task.WhenAll(indexTasks);
    }
}
