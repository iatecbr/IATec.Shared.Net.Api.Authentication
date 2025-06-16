using System.Text.Json.Serialization;

namespace IATec.Shared.Api.Authentication.Session;

public class Identity
{
    [JsonPropertyName("email")] public List<string>? EmailList { get; init; } = [];
}
