namespace IATec.Shared.Api.Authentication.Configurations.Options;

/// <summary>
/// Represents the configuration options for OAuth JWT authentication.
/// </summary>
public class OAuthJwtOption
{
    /// <summary>
    /// Gets the configuration section key used to bind these options.
    /// </summary>
    public const string Key = "OAuthJwt";

    /// <summary>
    /// Gets the authority URL of the token issuer.
    /// </summary>
    public required string Authority { get; init; }

    /// <summary>
    /// Gets the project identifier used to validate the audience claim.
    /// </summary>
    public required string ProjectId { get; init; }
}
