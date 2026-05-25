using IATec.Shared.Api.Authentication.Configurations.Options;
using IATec.Shared.Api.Authentication.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IATec.Shared.Api.Authentication.Extension;

/// <summary>
/// Provides extension methods for configuring JWT authentication in ASP.NET Core applications.
/// </summary>
public static class AuthenticationExtension
{
    /// <summary>
    /// Adds JWT bearer authentication and registers related services.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    /// <param name="configuration">The application configuration used to read <see cref="OAuthJwtOption"/> settings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection(OAuthJwtOption.Key)
            .Get<OAuthJwtOption>();

        if (jwtOptions is null)
            throw new ArgumentNullException(nameof(configuration),
                $"The '{OAuthJwtOption.Key}' configuration section is missing. Ensure both '{OAuthJwtOption.Key}:Authority' and '{OAuthJwtOption.Key}:ProjectId' are set in appsettings.json.");

        if (string.IsNullOrWhiteSpace(jwtOptions.Authority))
            throw new ArgumentNullException(nameof(jwtOptions.Authority),
                $"'{OAuthJwtOption.Key}:Authority' is required and cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(jwtOptions.ProjectId))
            throw new ArgumentNullException(nameof(jwtOptions.ProjectId),
                $"'{OAuthJwtOption.Key}:ProjectId' is required and cannot be null or empty.");

        var authority = $"{jwtOptions.Authority}{jwtOptions.ProjectId}";

        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.ProjectId,
                    ValidateLifetime = true,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<UserContextFirebase>();

        return services;
    }

    /// <summary>
    /// Configures the HTTP request pipeline to use authentication and authorization.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    /// <returns>The updated web application.</returns>
    public static WebApplication UseAuthenticationConfiguration(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
