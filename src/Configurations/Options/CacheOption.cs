namespace IATec.Shared.Api.Authentication.Configurations.Options;

public class CacheOptions
{
    public const string Key = "Cache";
    public string InstanceName { get; init; } = string.Empty;
    public int Expiration { get; init; } = 15;
    public string RedisConnection { get; init; } = string.Empty;
}
