# Products App Submission

This repository contains:
- `backend/` -> .NET 8 Products API (JWT auth, SQL Server via EF Core, unit + integration tests)
- `frontend/` -> React admin portal (login, products table, add-product modal, filter controls)

## Architecture
- See [ARCHITECTURE.md](./ARCHITECTURE.md) for the event-driven microservices diagram.
- Rendered image is available at [docs/architecture-diagram.png](./docs/architecture-diagram.png).

## Run Backend
```powershell
cd backend
dotnet restore
dotnet build
dotnet run
```

## Run Frontend
```powershell
cd frontend
npm.cmd install
npm.cmd run dev
```

## Test Backend
```powershell
cd backend
dotnet test Test.sln
```
