namespace IATec.Shared.Api.Authentication.Configurations.Options;

public class OAuthJwtOption
{
    public const string Key = "OAuthJwt";
    public string Authority { get; init; } = string.Empty;
    public string ProjectId { get; init; } = string.Empty;
}