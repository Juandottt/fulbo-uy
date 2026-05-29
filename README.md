# FulboUY

FulboUY is a web application for organizing amateur football matches. The backend exposes a REST API for authentication, player profiles, matches, team balancing, cost splitting, and payment tracking. A React frontend is present and in progress.

## Stack

- Backend: .NET 8, ASP.NET Core Web API
- Architecture: Clean Architecture style with API, Application, Domain, and Infrastructure projects
- Data access: Entity Framework Core Code First
- Database: SQL Server
- Authentication: JWT Bearer tokens, BCrypt password hashing
- Frontend: React 18, Vite, Tailwind CSS
- Tests: MSTest, Moq, FluentAssertions
- DevOps: Docker, Docker Compose

## Current Status

- Authentication: implemented
- Player profiles: implemented
- Match creation/listing/joining: implemented
- Team balancing: implemented
- Cost split and payment status APIs: implemented
- React frontend: in progress
- Product CRUD / US15: planned, not implemented in the current codebase
- Deployment: To be defined

## Run Backend

From the repository root:

```powershell
dotnet restore FulboUY.sln
dotnet run --project src/FulboUY.API/FulboUY.API.csproj
```

Default local API URL: `http://localhost:5089`

Swagger is available in Development at:

```txt
http://localhost:5089/swagger
```

## Run Frontend

From the repository root:

```powershell
cd client
npm install
npm run dev
```

Default frontend URL:

```txt
http://localhost:5173
```

## Run Tests

From the repository root:

```powershell
dotnet test FulboUY.sln
```

Current test baseline:

- Passed: 19
- Failed: 0
- Skipped: 0

## Docker

```powershell
docker-compose up --build
```

Docker configuration exists for the API, frontend, and SQL Server. Runtime environment details should be reviewed in `.env.example` before use.

## Useful Commands

```powershell
git status
git log --oneline -5
dotnet restore FulboUY.sln
dotnet build FulboUY.sln
dotnet test FulboUY.sln
dotnet run --project src/FulboUY.API/FulboUY.API.csproj
```

Frontend:

```powershell
cd client
npm install
npm run dev
npm run build
```

## Documentation

Project documentation lives in `docs/`:

- `00_PROJECT_OVERVIEW.md`
- `01_PDR.md`
- `02_TECH_STACK.md`
- `03_ARCHITECTURE.md`
- `04_GIT_WORKFLOW.md`
- `05_USER_STORIES.md`
- `06_ROADMAP.md`
- `07_DEFINITION_OF_DONE.md`
- `08_API_SPEC.md`
- `09_TEST_STRATEGY.md`
- `10_AGENT_WORKFLOW.md`
- `11_DECISIONS.md`
- `12_BACKLOG.md`
