# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [2.0.0] - 2026-05-21

### BREAKING CHANGES
- `OAuthJwtOption.Authority` and `OAuthJwtOption.ProjectId` changed from `string` (with `string.Empty` default) to `required string`.
- `Identity.EmailList` changed from `List<string>?` to `List<string>` (removed the nullable annotation).
- `UserContextFirebase.GetEmail()` now uses `First()` instead of `FirstOrDefault()` internally; callers should ensure the e-mail list is not empty.

### ADDED
- Complete XML documentation (summaries) added to all public classes, methods, and properties to improve IntelliSense support across consuming projects.
- `GetPerson()` method introduced in `UserContextFirebase` to retrieve `PersonDto` claims.

### CHANGED
- Updated Microsoft packages from `10.0.1` to `10.0.8`:
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
  - `Microsoft.Extensions.Configuration.Abstractions`
  - `Microsoft.Extensions.Configuration.Binder`
  - `Microsoft.Extensions.DependencyInjection.Abstractions`
- Removed null-forgiving operator (`!`) from `UserContextFirebase.GetEmail()` to enforce explicit null handling.
- Reordered members inside `UserContextFirebase` for clarity.

---

## [1.1.0] - 2026-01-09

### ADDED
- Added description to the NuGet package metadata.

### CHANGED
- Updated all dependencies to 10.x versions.
- Target framework upgraded to .NET 10.0.

---

## [1.0.0] - 2025-08-12

### ADDED
- Initial stable release of the authentication library.
- JWT Bearer authentication configured for Firebase tokens.
- `UserContextFirebase` helper class to extract user claims (email, UID, provider type).
- `AuthenticationExtension` methods for easy `IServiceCollection` and `WebApplication` registration.

---

## [0.1.2] - 2025-07-17

### CHANGED
- Finalized library setup and configuration binding corrections.

### FIXED
- Corrected configuration file binding for `appsettings.json` usage.

---
