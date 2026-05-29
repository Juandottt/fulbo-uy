# Decision Log

Use this file for lightweight architecture and product decisions.

## 2026-04-17

Decision: Use .NET 8, ASP.NET Core Web API, React, SQL Server, EF Core, and Docker.

Reason: The repository was initialized with this stack and project structure.

Consequences:

- Backend development follows .NET conventions.
- SQL Server is the primary database target.
- Frontend development uses React/Vite.

## 2026-04-17

Decision: Use a Clean Architecture style with API, Application, Domain, and Infrastructure projects.

Reason: The codebase separates HTTP concerns, business logic, domain entities, and persistence.

Consequences:

- Controllers should stay thin.
- Business rules belong in Application services.
- Infrastructure implements persistence.
- Domain should remain infrastructure-independent.

## 2026-04-24

Decision: Use JWT Bearer authentication with role-based authorization.

Reason: The API requires authenticated player/admin workflows.

Consequences:

- Protected endpoints require JWT tokens.
- Admin-only endpoints use role authorization.
- JWT settings must be configured safely per environment.

## 2026-04-25

Decision: Use MSTest, Moq, and FluentAssertions for Application service unit tests.

Reason: The test project was added with these dependencies and focuses on business logic.

Consequences:

- New business logic should be covered with Application unit tests.
- Repository dependencies should be mocked.

## To Be Defined

Decision: Product CRUD / US15 requirements.

Reason: Acceptance criteria are not present in the current codebase.

Consequences:

- Product implementation should not start until required fields, validation, authorization, and endpoint behavior are defined.
