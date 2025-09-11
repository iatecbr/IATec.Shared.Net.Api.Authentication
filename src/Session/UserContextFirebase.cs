using IATec.Shared.Api.Authentication.DTOs;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace IATec.Shared.Api.Authentication.Session;

public class UserContextFirebase(IHttpContextAccessor httpContextAccessor)
{
    private const string FirebaseClaimType = "firebase";

    public string GetEmail()
    {
        var claim = GetClaim(FirebaseClaimType);
        var firebaseClaim = JsonSerializer.Deserialize<FirebaseClaim>(claim);
        if (firebaseClaim?.Identity?.EmailList != null)
            return firebaseClaim.Identity?.EmailList.FirstOrDefault()!;

        throw new ArgumentNullException($"{nameof(FirebaseClaim)} cannot be null!");
    }

    public string GetUId()
    {
        return GetClaim("user_id");
    }

    public string GetProviderType()
    {
        var claim = GetClaim(FirebaseClaimType);
        var firebaseClaim = JsonSerializer.Deserialize<FirebaseClaim>(claim);
        if (firebaseClaim?.ProviderType != null)
            return firebaseClaim.ProviderType;

        throw new ArgumentNullException($"{nameof(FirebaseClaim)} cannot be null!");
    }

    private string GetClaim(string claimType)
    {
        var claim = httpContextAccessor.HttpContext?
            .User
            .Claims
            .FirstOrDefault(c => c.Type.Equals(claimType))?.Value;

        return claim ?? string.Empty;
    }

    public object? GetPerson()
    {        
        var personId = GetClaim("PersonId");

        if (personId == null || personId == "")
            return null;
        
        return new PersonDto(personId, GetClaim("FirstName"), GetClaim("LastName"));
    }
}
