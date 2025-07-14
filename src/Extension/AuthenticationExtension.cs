using IATec.Shared.Api.Authentication.Configurations.Options;
using IATec.Shared.Api.Authentication.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using IATec.Shared.Api.Authentication.Services;

namespace IATec.Shared.Api.Authentication.Extension;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection(OAuthJwtOption.Key)
            .Get<OAuthJwtOption>();

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
                        var tokenStore = context.HttpContext.RequestServices.GetRequiredService<TokenStoreService>();
                        await tokenStore.StoreTokenAsync(token, "token-validated");
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