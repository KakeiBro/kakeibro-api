using Redis.OM.Modeling;

namespace Modules.Authentication.Data;

[Document(StorageType = StorageType.Json, Prefixes = ["User"], IndexName = "users")]
public class User
{
    [RedisIdField]
    [Indexed]
    public int UserId { get; set; }

    [Indexed]
    public string? FirstName { get; set; }

    [Indexed]
    public string? LastName { get; set; }

    [Searchable]
    public string Email { get; set; } = string.Empty;

    [Indexed]
    public bool Deleted { get; set; }

    [Indexed]
    public IList<Token> Tokens { get; init; } = [];

    [Indexed]
    public IList<MfaDevice> MfaDevices { get; init; } = [];
}
