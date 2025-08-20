using IATec.Shared.Api.Authentication.Configurations.Options;
using IATec.Shared.Api.Authentication.Session;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IATec.Shared.Api.Authentication.Extension;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection(OAuthJwtOption.Key)
            .Get<OAuthJwtOption>();

        var cacheOptions = configuration
            .GetSection(CacheOptions.Key)
            .Get<CacheOptions>();

        services.AddCacheConfiguration(configuration);

        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                var authority = $"{jwtOptions?.Authority}{jwtOptions?.ProjectId}";
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions?.ProjectId,
                    ValidateLifetime = true,
                    RoleClaimType = ClaimTypes.Role
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var token = context.SecurityToken.Id;
                        var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                        var entryOptions = new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheOptions!.Expiration)
                        };

                        await cache.SetStringAsync(token, "token-validated", entryOptions);
                    }
                };
            });            

        services.AddHttpContextAccessor();
        services.AddScoped<UserContextFirebase>();

        return services;
    }

    public static WebApplication UseAuthenticationConfiguration(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}