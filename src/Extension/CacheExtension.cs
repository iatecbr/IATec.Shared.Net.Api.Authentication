using IATec.Shared.Api.Authentication.Configurations.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IATec.Shared.Api.Authentication.Extension;

public static class CacheExtension
{
    public static IServiceCollection AddCacheConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var cacheOptions = configuration
            .GetSection(CacheOptions.Key)
            .Get<CacheOptions>();

        if (cacheOptions!.UseDistributedCache)
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheOptions.RedisConnection;
            });
        }

        return services;
    }

    public static WebApplication UseCacheConfiguration(this WebApplication app)
    {
        return app;
    }
}
