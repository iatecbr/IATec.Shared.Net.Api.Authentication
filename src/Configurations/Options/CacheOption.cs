namespace IATec.Shared.Api.Authentication.Configurations.Options;

public class CacheOption
{
    public const string Key = "Cache";
    public string InstanceName { get; init; } = "Local";
    public int Expiration { get; init; } = 15;
    public string RedisConnection { get; init; } = "localhost:6379,password=,ssl=false,abortConnect=false,connectTimeout=10000,defaultDatabase=0";
    public bool UseDistributedCache { get; init; } = false;
}
