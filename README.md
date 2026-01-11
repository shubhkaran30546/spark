# Spark

Full-stack small e-commerce computer app with an ASP.NET Core backend and an Angular frontend.

## Project Structure
- `backend/` – ASP.NET Core + EF Core API
- `spark-ui/` – Angular frontend (Universal / SSR enabled)

---

## Quick Overview
- API controllers: `backend/Controllers`
- EF Core models: `backend/Models` and `backend/Data/ApplicationDbContext.cs`
- Swagger for API exploration (enabled in Development)
- Angular app: `spark-ui/src` (standalone components + SSR)

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js + npm](https://nodejs.org/)
- (optional) Angular CLI:

```bash
npm install -g @angular/cli
```

## Backend (ASP.NET Core)

- Requirements: .NET 8 SDK installed.
- From the repo root, start the backend:

```bash
cd backend
dotnet restore
dotnet ef database update    # optional: apply migrations
dotnet run
```

- By default the project launches HTTP on `http://localhost:5097` (see `backend/Properties/launchSettings.json`).
- Swagger is available in Development at `http://localhost:5097/swagger`.

Notes:
- The app uses Identity for authentication; auth endpoints live under `/api/auth`.
- For local SSR development, prefer using the HTTP backend URL from Node to avoid TLS cert issues.

## Frontend (Angular)

- Requirements: Node.js and npm (see `package.json` for tested versions).
- Install dependencies and run in development:

```bash
cd spark-ui
npm install
npm run start
```

- To build server-side rendering (SSR) output and run the Node server:

```bash
cd spark-ui
npm run build
npm run serve:ssr:spark-ui
```

- The SSR server will fetch backend APIs. If your backend runs on a different port or protocol, update `src/app/services/computer.service.ts` accordingly.

## Troubleshooting

- NG02801 (fetch warning): Ensure `provideHttpClient(withFetch())` is registered in `src/app/app.config.server.ts` (already configured here).
- ERR_SSL_PACKET_LENGTH_TOO_LONG / fetch failed: This happens when Node's fetch tries HTTPS against an HTTP backend port. Use `http://localhost:5097` for SSR or configure valid TLS for Node.
- CORS: Backend allows `http://localhost:4200` by default. If your frontend uses a different origin, update CORS policy in `backend/Program.cs`.

## Development tips

- When changing models/migrations, run EF migrations and update the DB with `dotnet ef database update`.
- Swagger JSON is useful for quick API checks: `http://localhost:5097/swagger/v1/swagger.json`.

## Contributing / Pushing

- Make sure you have a Git remote configured. Example:

```bash
git remote add origin <your-repo-url>
git push -u origin main
```

---

If you want, I can also add a short `Makefile` or npm script to automate starting backend + SSR together.
