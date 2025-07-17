using IATec.Shared.Api.Authentication.Configurations.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace IATec.Shared.Api.Authentication.Services;
public class TokenStoreService(IDistributedCache cache, IOptions<CacheOptions> cacheOptions)
{    
    public async Task StoreTokenAsync(string token, string value)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheOptions.Value.Expiration)
        };

        await cache.SetStringAsync(token, value, options);
    }
}