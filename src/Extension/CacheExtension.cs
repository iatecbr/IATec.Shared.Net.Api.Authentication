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

        if (cacheOptions!.RedisConnection != "")
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheOptions.RedisConnection;
                options.InstanceName = cacheOptions.InstanceName;
            });
        }

        else services.AddDistributedMemoryCache();

        return services;
    }

    public static WebApplication UseCacheConfiguration(this WebApplication app)
    {
        return app;
    }
}
