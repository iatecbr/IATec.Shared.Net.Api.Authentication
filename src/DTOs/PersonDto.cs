namespace IATec.Shared.Api.Authentication.DTOs;

/// <summary>
/// Represents a person with basic identification details.
/// </summary>
/// <param name="PersonId">The unique identifier of the person.</param>
/// <param name="FirstName">The first name of the person.</param>
/// <param name="LastName">The last name of the person.</param>
public record PersonDto(string PersonId, string FirstName, string LastName);
