# MarketPlace API

ASP.NET Core Web API · .NET 10 · EF Core · SQL Server · Repository Pattern · Service Pattern · Scalar

## Run with Docker

Requirements: Docker Engine / Docker Desktop with Docker Compose.

```bash
cp .env.example .env

docker compose up --build
```

- API: http://localhost:8080
- Scalar: http://localhost:8080/scalar
- Health: http://localhost:8080/health
- SQL Server: localhost:14333

The development API applies the included `InitialCreate` migration on startup. SQL Server can take several seconds to initialize. The container retries temporary connection failures.

## Run locally

Requires .NET 10 SDK and an accessible SQL Server instance.

```bash
cp .env.example .env

docker compose up -d sqlserver

dotnet tool restore

dotnet restore MarketPlace.sln

dotnet ef database update --project MarketPlace.Api

dotnet run --project MarketPlace.Api --launch-profile http
```

- Scalar: http://localhost:5080/scalar
- OpenAPI JSON: http://localhost:5080/openapi/v1.json

The local connection string is configured in `MarketPlace.Api/appsettings.json`. For other databases, override `ConnectionStrings__DefaultConnection` via the environment. The passwords in the example configuration are for local development only.

## Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/api/users` | Create a buyer or seller |
| GET | `/api/users/{id}` | Get user |
| POST | `/api/stores` | Create a seller's store |
| GET | `/api/stores` | List stores |
| GET | `/api/stores/{id}` | Get store |
| GET | `/api/stores/{id}/products` | List store products |
| POST | `/api/categories` | Create category |
| GET | `/api/categories` | List categories |
| POST | `/api/products` | Add product to owned store |
| GET | `/api/products` | Search/filter/paginate products |
| GET | `/api/products/{id}` | Get product |
| POST | `/api/orders` | Create order and deduct stock |
| GET | `/api/users/{id}/orders` | Get buyer orders |

`GET /api/products` accepts `search`, `categoryId`, `storeId`, `minPrice`, `maxPrice`, `page` and `pageSize` (1–100).

## Verification

The request collection is `MarketPlace.Api/MarketPlace.Api.http`.

An automated end-to-end smoke test is included:

```bash
python scripts/smoke_test.py http://localhost:8080
```

Create seller and buyer → create store → create category → create product → list stores → search product → create order → check remaining stock → check buyer orders.

On a fresh database, the HTTP examples use seller ID 1, buyer ID 2, store ID 1, category ID 1 and product ID 1. Use response IDs if data already exists.

## Scope

A single order contains one product and one quantity, as defined by the assignment. Stock deduction uses a conditional SQL update inside the same database transaction as order creation.

This is an educational project, not a production-ready authenticated marketplace. Client-supplied user IDs are not proof of identity. Before public deployment, add authentication, authorization based on verified identity, rate limiting, idempotent order requests, audit logging, and controlled database migrations. Do not use the bundled development database password in production.
