# Vector

Backend solution for the Vector storefront.

## Structure

- **Vector.API** — ASP.NET Core Web API (controllers, HTTP concerns).
- **Vector.Application** — application/business logic (use cases, services).
- **Vector.Domain** — domain entities and core business rules.
- **Vector.Infrastructure** — data access and external integrations.

The web frontend lives in a separate `web` folder alongside this solution.

## Getting started

```
dotnet restore
dotnet build
dotnet run --project Vector.API
```
