# Vector — Pro Gaming Merch Store

A storefront for a fictional pro esports organization — product catalog, categories (Pro Kit, Apparel, Hardware, Accessories, Bundles, Legacy), cart, and checkout. This repo is the backend API; the frontend below is the design target.

![Vector storefront preview](docs/storefront-preview.png)

## Tech Stack

**Backend**
- ASP.NET Core 10 Web API
- Clean Architecture (Domain, Application, Infrastructure, API)
- JWT Bearer authentication
- Swagger / OpenAPI (Swashbuckle) with an Authorize flow

**Frontend**
- Not started yet — will live in a `web/` folder alongside `src/` once work begins.

## Project Structure

```
Vector/
├── src/
│   ├── Vector.API             # Controllers, Swagger/auth setup, Program.cs
│   ├── Vector.Application     # Use cases, DTOs, interfaces (empty scaffold)
│   ├── Vector.Domain          # Entities, value objects, domain logic (empty scaffold)
│   └── Vector.Infrastructure  # EF Core, repositories, external services (empty scaffold)
├── tests/
│   ├── Vector.UnitTests
│   └── Vector.IntegrationTests
├── docs/
├── README.md
└── Vector.slnx
```

## Status

Early scaffold. `Vector.Domain`, `Vector.Application`, and `Vector.Infrastructure` are empty class libraries wired into the solution and ready to build out. `Vector.API` currently exposes a `ProductsController` backed by in-memory sample data, with JWT bearer auth and Swagger UI already configured.

## Roadmap

- Product catalog with category filtering
- Product detail page
- Shopping cart (add, remove, update quantity)
- Checkout flow
- Real JWT issuance (login/register) — auth middleware is in place, token issuing isn't yet
- Admin: product management (CRUD)
- Persistence via EF Core (Infrastructure layer)
- `web/` frontend

## Getting Started

### Prerequisites
- .NET 10 SDK

### Backend

```bash
dotnet restore
dotnet build
dotnet run --project src/Vector.API
```

API runs on `https://localhost:7055` (`http://localhost:5035`), with Swagger UI at `/swagger`.

### Tests

```bash
dotnet test
```
