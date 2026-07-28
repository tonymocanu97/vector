# Vector

Full-stack e-commerce storefront for a fictional pro esports organization. Product catalog, authentication, cart, checkout, and order history.

[![Backend CI](https://github.com/tonymocanu97/vector/actions/workflows/backend-ci.yml/badge.svg)](https://github.com/tonymocanu97/vector/actions/workflows/backend-ci.yml)
[![Frontend CI](https://github.com/tonymocanu97/vector/actions/workflows/frontend-ci.yml/badge.svg)](https://github.com/tonymocanu97/vector/actions/workflows/frontend-ci.yml)

**Live:** https://vector-web-ruby.vercel.app/

![Vector storefront preview](docs/storefront-preview.png)

## Tech Stack

**Backend:** ASP.NET Core · Clean Architecture · EF Core · PostgreSQL · JWT Auth  
**Frontend:** Next.js · React · TypeScript · Tailwind CSS  
**Testing:** xUnit · Moq · FluentAssertions · Testcontainers  
**DevOps:** Docker · GitHub Actions · Railway · Vercel  

## Features

- JWT authentication with BCrypt password hashing
- Product catalog with category filtering
- Cart with stock validation
- Checkout with order history
- Admin product CRUD (role-gated)
- Newsletter signup
- Scripted FAQ chat widget (client-side keyword matching, no AI/backend calls)

## Project Structure

```
Vector/
├── src/
│   ├── Vector.API
│   ├── Vector.Application
│   ├── Vector.Domain
│   ├── Vector.Infrastructure
│   └── Vector.Web
├── tests/
│   ├── Vector.UnitTests
│   └── Vector.IntegrationTests
├── .github/workflows/
├── docs/
├── Dockerfile
├── README.md
└── Vector.slnx
```

## Getting Started

### Backend

```bash
dotnet restore
dotnet ef database update --project src/Vector.Infrastructure --startup-project src/Vector.API
dotnet run --project src/Vector.API
```

API runs on https://localhost:7055 (http://localhost:5035), with Swagger UI at /swagger.

### Frontend

```bash
cd src/Vector.Web
npm install
npm run dev
```

Runs on `http://localhost:3000`. Defaults to `http://localhost:5035/api` if `NEXT_PUBLIC_API_URL` isn't set.

## Testing

```bash
# Unit tests only
dotnet test tests/Vector.UnitTests

# Integration tests (spins up a real Postgres container - Docker must be running)
dotnet test tests/Vector.IntegrationTests
```