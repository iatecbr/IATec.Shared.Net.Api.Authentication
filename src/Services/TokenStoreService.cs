using Microsoft.Extensions.Caching.Distributed;

namespace IATec.Shared.Api.Authentication.Services;
public class TokenStoreService
{
    private readonly IDistributedCache _cache;

    public TokenStoreService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task StoreTokenAsync(string token, string value)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        };

        await _cache.SetStringAsync(token, value, options);
    }
}