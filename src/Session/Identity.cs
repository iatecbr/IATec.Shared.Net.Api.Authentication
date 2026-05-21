using System.Text.Json.Serialization;

namespace IATec.Shared.Api.Authentication.Session;

/// <summary>
/// Represents the identity information extracted from a Firebase claim.
/// </summary>
public class Identity
{
    /// <summary>
    /// Gets the list of emails associated with the identity.
    /// </summary>
    [JsonPropertyName("email")] public List<string> EmailList { get; init; } = [];
}
