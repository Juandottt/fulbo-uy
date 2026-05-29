# AGENTS.md

Instructions for AI coding agents working in this repository.

## Core Rules

- Inspect the repository before editing.
- Make small, focused changes.
- Do not modify source code unless the user explicitly asks for implementation.
- Do not modify tests unless the task requires test work.
- Do not change package files unless dependency changes are explicitly approved.
- Do not refactor large areas without approval.
- Preserve existing naming, folder structure, public APIs, and conventions.
- Explain important decisions briefly.
- If a requirement is unclear, write `To be defined` in documentation or ask before changing behavior.

## Architecture Boundaries

The project follows a Clean Architecture style:

- `src/FulboUY.Domain`: domain entities and enums only.
- `src/FulboUY.Application`: business logic, use cases, DTOs, and repository/service interfaces.
- `src/FulboUY.Infrastructure`: EF Core DbContext, migrations, and repository implementations.
- `src/FulboUY.API`: controllers, HTTP DTOs, middleware, authentication, DI setup.
- `client`: React frontend.
- `tests`: automated tests.

Dependency direction:

```txt
API -> Application -> Domain
Infrastructure -> Application + Domain
```

Rules:

- Controllers must stay thin.
- Business rules belong in Application services.
- Infrastructure handles persistence only.
- Domain must not depend on API, Infrastructure, EF Core, SQL Server, or React.
- API input/output should use DTOs.

## Safe Commands

These commands are generally safe for inspection or verification:

```powershell
git status
git diff
git log --oneline -5
rg "search-term"
dotnet restore FulboUY.sln
dotnet build FulboUY.sln
dotnet test FulboUY.sln
npm --prefix client run build
```

## Commands Requiring Approval

Ask before running commands that change repository history, delete files, alter databases, install dependencies, or publish changes:

```powershell
git commit
git push
git reset
git clean
git checkout
git merge
dotnet ef migrations add
dotnet ef database update
npm install
Remove-Item
del
rmdir
```

## Git Rules

- Do not commit without explicit user approval.
- Do not push without explicit user approval.
- Do not commit directly to `main` or `develop`.
- Use feature branches for implementation work.
- Use docs branches for documentation-only work.
- Use Conventional Commits:
  - `docs:` documentation-only changes
  - `test:` tests
  - `feat:` new behavior
  - `fix:` bug fixes
  - `refactor:` behavior-preserving improvements
  - `chore:` maintenance/configuration

## Testing Rules

- Current backend test stack: MSTest, Moq, FluentAssertions.
- Focus tests on Application/business logic.
- Use TDD for complex features.
- Do not remove or weaken tests to make a build pass.
- Run `dotnet test FulboUY.sln` after meaningful backend changes.
- For frontend changes, run the relevant npm build/test command if available.

## Secrets And Environment

- Do not expose secrets.
- Do not print real connection strings, passwords, API keys, JWT secrets, tokens, or certificates.
- Use `.env.example` for safe placeholders only.
- Treat `.env`, `appsettings.Development.json`, and local database credentials as sensitive.

## Product CRUD / US15 Notes

Product CRUD / US15 is planned but not implemented in the current codebase.

Recommended implementation order when approved:

1. Product business requirements and validation rules.
2. Failing Application tests.
3. Domain entity.
4. Repository and service interfaces.
5. Application service.
6. EF Core DbSet/configuration and repository.
7. API DTOs and controller.
8. DI registration.
9. Migration.
10. Full test run.
