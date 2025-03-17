using Redis.OM.Modeling;

namespace Modules.Authentication.Data;

[Document(StorageType = StorageType.Json)]
public class Token
{
    [RedisIdField]
    [Indexed]
    public int TokenId { get; set; }

    [Indexed]
    public string Value { get; set; } = string.Empty;

    [Indexed]
    public string Vendor { get; set; } = string.Empty;

    [Indexed]
    public long GracePeriod { get; set; }

    [Indexed]
    public bool Deleted { get; set; }
}
