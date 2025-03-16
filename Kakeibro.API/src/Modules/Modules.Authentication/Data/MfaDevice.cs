using Redis.OM.Modeling;

namespace Modules.Authentication.Data;

[Document(StorageType = StorageType.Json)]
public class MfaDevice
{
    [RedisIdField]
    [Indexed]
    public int MfaDeviceId { get; set; }

    [Indexed]
    public string Name { get; set; } = string.Empty;

    [Indexed]
    public bool Deleted { get; set; }
}
