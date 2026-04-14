# ZentekLabs Senior Developer Coding Test Submission

This repository contains a complete solution for the coding test:

- .NET 6+ Products Web API (implemented with .NET 8)
- Anonymous health endpoint
- Secured product endpoints (JWT)
- Unit and integration tests
- React frontend (login + admin products panel)
- Simple event-driven microservices architecture diagram

---

## Project Structure

- `backend/` - ASP.NET Core Web API + tests
- `frontend/` - React admin portal
- `ARCHITECTURE.md` - Mermaid architecture diagram
- `docs/architecture-diagram.png` - rendered architecture image

---

## Backend Features (`backend/`)

### API Endpoints

- `GET /health` (anonymous)
- `POST /auth/token` (anonymous, returns JWT)
- `POST /products` (secured)
- `GET /products` (secured)
- `GET /products?colour={colour}` (secured)

### Security

- JWT Bearer authentication
- Demo credentials from app settings:
  - Username: `testuser`
  - Password: `P@ssw0rd123`

### Persistence

- Entity Framework Core + SQL Server
- Product data stored in SQL Server (`Products` table)
- DB schema initialized at startup (`EnsureCreated()`)

### Tests

- Unit tests: product service behavior
- Integration tests:
  - health endpoint
  - unauthorized access protection
  - authenticated create/list/filter flow

Run tests:
```powershell
cd backend
dotnet test Test.sln
