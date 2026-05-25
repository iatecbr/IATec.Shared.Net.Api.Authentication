using System.Text.Json.Serialization;

namespace IATec.Shared.Api.Authentication.Session;

/// <summary>  
/// Represents the Firebase-specific claims extracted from an authentication token.  
/// </summary>  
public class FirebaseClaim
{  
    /// <summary>  
    /// Gets the identity information associated with the Firebase user.  
    /// </summary>  
    [JsonPropertyName("identities")] public Identity? Identity { get; init; }  

    /// <summary>  
    /// Gets the authentication provider type (e.g., Google, email/password).  
    /// </summary>  
    [JsonPropertyName("sign_in_provider")] public string? ProviderType { get; init; }  
}
