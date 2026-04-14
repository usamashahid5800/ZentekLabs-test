# Products App (Backend + Frontend)

This repository now includes:

- Anonymous health endpoint: `GET /health`
- JWT-secured product endpoints:
  - `POST /products`
  - `GET /products`
  - `GET /products?colour=blue`
- SQL Server persistence with Entity Framework Core
- Unit and integration tests
- React frontend client in `frontend/`

## Project structure

- `d:\Test` -> .NET API
- `d:\Test\frontend` -> React app

## Configuration

Update your SQL Server connection string in [appsettings.json](/d:/Test/appsettings.json):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ProductsDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

`Products` table is created automatically on startup via `EnsureCreated()`.

## Run API

```powershell
dotnet restore
dotnet build
dotnet run
```

Swagger opens at `/swagger` in development (default API URL: `https://localhost:7236`).

## Run frontend

```powershell
cd frontend
npm.cmd install
npm.cmd run dev
```

Frontend dev URL: `http://localhost:5173`

## Test credentials

Use `POST /auth/token` with:

```json
{
  "username": "testuser",
  "password": "P@ssw0rd123"
}
```

Use returned `accessToken` as `Bearer <token>` for `/products` endpoints.

## Tests

```powershell
dotnet test Test.sln
```

## Build frontend

```powershell
cd frontend
npm.cmd run build
```
