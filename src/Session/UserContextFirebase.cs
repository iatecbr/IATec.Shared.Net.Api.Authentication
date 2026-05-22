using IATec.Shared.Api.Authentication.DTOs;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace IATec.Shared.Api.Authentication.Session;

/// <summary>
/// Provides access to Firebase-related user claims from the current HTTP context.
/// </summary>
public class UserContextFirebase(IHttpContextAccessor httpContextAccessor)
{
    private const string FirebaseClaimType = "firebase";

    /// <summary>
    /// Retrieves the user's email from the Firebase claim.
    /// </summary>
    /// <returns>The primary email address of the authenticated user.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the Firebase claim is missing or invalid.</exception>
    public string GetEmail()
    {
        var claim = GetClaim(FirebaseClaimType);
        var firebaseClaim = JsonSerializer.Deserialize<FirebaseClaim>(claim);
        if (firebaseClaim?.Identity?.EmailList is not null && firebaseClaim.Identity.EmailList.Count > 0)
            return firebaseClaim.Identity.EmailList.First();

        throw new ArgumentNullException($"{nameof(FirebaseClaim)} cannot be null!");
    }

    /// <summary>
    /// Retrieves the user's unique identifier from the claims.
    /// </summary>
    /// <returns>The user identifier, or <see cref="string.Empty"/> if not found.</returns>
    public string GetUId()
    {
        return GetClaim("user_id");
    }

    /// <summary>
    /// Retrieves the authentication provider type from the Firebase claim.
    /// </summary>
    /// <returns>The sign-in provider name (e.g., google.com, password).</returns>
    /// <exception cref="ArgumentNullException">Thrown when the Firebase claim is missing or invalid.</exception>
    public string GetProviderType()
    {
        var claim = GetClaim(FirebaseClaimType);
        var firebaseClaim = JsonSerializer.Deserialize<FirebaseClaim>(claim);
        if (firebaseClaim?.ProviderType != null)
            return firebaseClaim.ProviderType;

        throw new ArgumentNullException($"{nameof(FirebaseClaim)} cannot be null!");
    }

    /// <summary>
    /// Retrieves the person information from the claims.
    /// </summary>
    /// <returns>A <see cref="PersonDto"/> containing the person's details, or <c>null</c> if not present.</returns>
    public PersonDto? GetPerson()
    {        
        var personId = GetClaim("PersonId");

        return string.IsNullOrEmpty(personId)
            ? null
            : new PersonDto(personId, GetClaim("FirstName"), GetClaim("LastName"));
    }

    /// <summary>
    /// Retrieves a specific claim value by its type from the current HTTP context.
    /// </summary>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>The claim value, or <see cref="string.Empty"/> if not found.</returns>
    private string GetClaim(string claimType)
    {
        var claim = httpContextAccessor.HttpContext?
            .User
            .Claims
            .FirstOrDefault(c => c.Type.Equals(claimType))?.Value;

        return claim ?? string.Empty;
    }
}
