namespace Modules.Authentication.Config;

public class RedisConfig
{
    public string? EndpointUri { get; set; }

    public int Port { get; set; }

    public string? EndpointConnectionString { get; set; }

    public bool CleanOnShutdown { get; set; }

    public bool ForceMigration { get; set; }

    public bool Seed { get; set; }

    public bool RedisOff { get; set; }
}
