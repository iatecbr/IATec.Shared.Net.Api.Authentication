using System.Text.Json.Serialization;

namespace IATec.Shared.Api.Authentication.Session;

public class FirebaseClaim
{
    [JsonPropertyName("identities")] public Identity? Identity { get; init; }

    [JsonPropertyName("sign_in_provider")] public string? ProviderType { get; init; }
}