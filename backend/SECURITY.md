# Security & Deployment Guidance

This document lists recommended secure configuration steps for deploying the backend in production.

- Secrets & Configuration
  - Do NOT store secrets (JWT keys, DB connection strings, API keys) in source control or `appsettings.json` committed to the repo.
  - Use environment variables (e.g. `ASPNETCORE_` prefixed) or `dotnet user-secrets` for local development.
  - In CI/CD, inject secrets from a secure vault (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault).

- JWT & Authentication
  - Keep `Jwt:Key` long and random; rotate keys periodically.
  - Prefer using short-lived access tokens and secure refresh tokens stored in HttpOnly, Secure cookies.
  - Avoid storing tokens in `localStorage` in the SPA to reduce XSS risk.
  - Implement token revocation or a refresh-token blacklist if immediate revocation is required.

- Cookies & CSRF
  - If using cookies for authentication, mark them `HttpOnly`, `Secure`, and `SameSite=Strict`/`Lax` as appropriate.
  - Protect state-changing endpoints from CSRF attacks (anti-forgery tokens, same-site cookies).

- CORS
  - Restrict CORS origins to the exact production origins for the SPA. Do not allow `*` in production.
  - Manage CORS settings per environment.

- Input Validation & DTOs
  - Use DTOs for incoming requests and validate `ModelState` to prevent overposting.
  - Do not bind EF entities directly as action parameters for create/update operations.

- Logging
  - Do not log secrets, tokens, passwords, or PII. Use structured logging and add filters to redact sensitive fields.
  - Use centralized logging in production (ELK, Seq, Azure Monitor) with secure access and retention policies.

- Error Handling
  - Use `UseExceptionHandler` in production (already applied) to avoid leaking stack traces.
  - Return appropriate Problem Details responses for clients.

- Rate Limiting & Brute Force
  - Enable Identity lockout options and rate-limiting for authentication endpoints.
  - Consider global request throttling via middleware or API gateway.

- HTTPS & HSTS
  - Enforce HTTPS and enable HSTS in production (already applied in Program.cs).
  - Ensure certificates are provisioned and rotated securely.

- Content Security Policy (CSP)
  - Serve a strong CSP header to mitigate XSS.

- Swagger
  - Ensure Swagger/UI is disabled in production or protected behind authentication.

- Dependencies & Vulnerability Scanning
  - Keep NuGet and npm dependencies up to date and run automated vulnerability scans.

- Static File Serving
  - Serve static SPA files only from the intended `wwwroot` folder and disable directory browsing.

- Additional
  - Consider a Web Application Firewall (WAF) and application-level monitoring/alerts.
  - Regularly run penetration tests and security reviews.

This file is a starting checklist — apply controls iteratively and integrate them into CI/CD pipelines.
