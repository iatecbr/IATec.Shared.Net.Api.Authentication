# IATec.Shared.Api.Authentication

Shared library for authentication and authorization in ASP.NET Core APIs. Provides JWT Bearer authentication integration for Firebase tokens, as well as convenient helpers to access user claims.

## Requirements

- .NET 10.0 or later

## Installation

Install the package via NuGet:

```bash
dotnet add package IATec.Shared.Api.Authentication
```

Or add the following reference to your `.csproj` file:

```xml
<PackageReference Include="IATec.Shared.Api.Authentication" Version="2.0.0" />
```

## Configuration

Add the required settings to your `appsettings.json`:

```json
{
  "OAuthJwt": {
    "Authority": "https://securetoken.google.com/",
    "ProjectId": "your-firebase-project-id"
  }
}
```

## Usage

### 1. Register services

In your `Program.cs` (or `Startup.cs`), call the extension method:

```csharp
using IATec.Shared.Api.Authentication.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add authentication and authorization
builder.Services.AddAuthenticationConfiguration(builder.Configuration);

// ... other registrations
```

### 2. Configure the HTTP pipeline

Still in `Program.cs`, add the middleware:

```csharp
var app = builder.Build();

app.UseAuthenticationConfiguration();

// ... other middleware
```

### 3. Access user context

Inject `UserContextFirebase` into your controllers or services to retrieve user claims:

```csharp
using IATec.Shared.Api.Authentication.Session;

public class MyService
{
    private readonly UserContextFirebase _userContext;

    public MyService(UserContextFirebase userContext)
    {
        _userContext = userContext;
    }

    public void DoWork()
    {
        var email = _userContext.GetEmail();
        var userId = _userContext.GetUId();
        var provider = _userContext.GetProviderType();
        var person = _userContext.GetPerson();
    }
}
```

## API

### `AuthenticationExtension`

Extension methods that wire up JWT Bearer authentication and associated services.

| Member | Description |
|--------|-------------|
| `AddAuthenticationConfiguration` | Adds JWT Bearer authentication, the HTTP context accessor, and `UserContextFirebase`. |
| `UseAuthenticationConfiguration` | Registers authentication and authorization middleware in the request pipeline. |

### `UserContextFirebase`

Provides typed access to Firebase claims in the current HTTP context.

| Member | Description |
|--------|-------------|
| `GetEmail()` | Returns the primary e-mail address from the Firebase claim. |
| `GetUId()` | Returns the unique user identifier (`user_id`). |
| `GetProviderType()` | Returns the sign-in provider used (e.g., `google.com`, `password`). |
| `GetPerson()` | Returns a `PersonDto` with `PersonId`, `FirstName`, and `LastName` when present in claims. |

### `OAuthJwtOption`

Configuration object bound from the `OAuthJwt` section.

| Property | Description |
|----------|-------------|
| `Authority` | Base URL of the token issuer. |
| `ProjectId` | Firebase project identifier used as the valid audience. |

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a detailed history of changes.

## License

© IATec Solutions. All rights reserved.
