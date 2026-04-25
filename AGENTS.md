# Project Context

Stack:
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQL Server
- React 18 (frontend)

Architecture:
- Clean Architecture with 4 layers:
  - WebApi
  - BusinessLogic
  - DataAccess
  - Domain

Rules:
- Controllers must be thin (no business logic)
- Business rules go in BusinessLogic
- DataAccess only handles persistence
- Domain must not depend on infrastructure
- Use DTOs for API input/output

Coding Guidelines:
- Follow SOLID principles
- Prefer clean, readable code over clever code
- Avoid duplication
- Use meaningful naming

Testing:
- Use MSTest + Moq
- Focus tests on business logic
- Use TDD for complex features

Git Workflow:
- Use feature branches
- Do not commit directly to main/develop
- Use Conventional Commits:
  - test: (failing test first)
  - feat: (implementation)
  - refactor: (improvement without behavior change)
  - fix: (bug fixes)

Important:
- Do NOT refactor large parts unless explicitly requested
- Prefer minimal, safe changes
- Always explain decisions