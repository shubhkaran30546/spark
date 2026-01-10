# spark

Small e-commerce sample app built with ASP.NET Core and EF Core.

Quick overview
- Minimal API controllers under `Controllers/` (also some MVC views).
- EF Core models in `Models/` and `Data/ApplicationDbContext.cs`.
- Swagger is configured for API exploration.

Prerequisites
- .NET 8 SDK (this project targets net8.0)

Getting started

1. Restore and build:

```bash
dotnet restore
dotnet build
```

2. Run the app (bind to a local port):

```bash
dotnet run --urls http://localhost:5002
```

3. Open Swagger UI:

http://localhost:5002/swagger

Notes
- The project uses SQLite by default; connection string is in `appsettings.json`.
- The `AccountController` currently contains a placeholder `GenerateJwt` method — replace with a proper JWT implementation if you need token auth.
- JSON reference cycles are ignored via `ReferenceHandler.IgnoreCycles` to avoid serialization errors for navigation properties.

Database initialization
- The app runs `DbInitializer.Initialize(...)` at startup to seed sample Computers and Components.

If push fails
- If `git push` fails due to missing remote or auth, set up your remote and credentials, then run:

```bash
git remote add origin <your-repo-url>
git push -u origin main
```

