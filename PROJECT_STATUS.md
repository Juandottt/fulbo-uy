# Project Status

Date: 2026-05-29

## Repository State

- Current branch: `main`
- Working tree: clean
- Latest commits:
  - `6034f24` - Merge pull request #1 from `Juandottt/fix/backend-stability`
  - `cbde2ec` - `test: add application service unit tests`
  - `882cd14` - `fix: corregir bugs críticos y agregar seguridad básica en FASE 1`
  - `75c6b4f` - `feat: portfolio setup - README, Docker configs, .env.example`

## Current Scope

The current codebase implements the backend foundation for FulboUY using .NET 8, ASP.NET Core Web API, EF Core, SQL Server, and a React frontend.

The implemented backend features are:

- JWT authentication.
- Player profile creation, lookup, and update.
- Match creation and listing.
- Match joining.
- Match participant listing.
- Team balancing.
- Cost split calculation.
- Payment confirmation at API/service level.

Product CRUD / US15 is not implemented in the current checkout.

## Implemented By Layer

### Domain

Implemented entities:

- `User`
- `PlayerProfile`
- `Match`
- `MatchParticipant`
- `InviteLink`

Implemented enums:

- `UserRole`
- `MatchStatus`

Missing for Product CRUD / US15:

- No `Product` entity exists.
- No Product-specific domain rules exist.

### Domain Tests

- No dedicated Domain test project exists.
- Existing tests are focused on application/business logic.

Missing for Product CRUD / US15:

- No Product domain tests.

### Data Access Interfaces

Repository interfaces currently live in `src/FulboUY.Application/Interfaces`.

Implemented interfaces:

- `IUserRepository`
- `IPlayerProfileRepository`
- `IMatchRepository`
- `IMatchParticipantRepository`
- `IInviteLinkRepository`

Missing for Product CRUD / US15:

- No `IProductRepository`.

### Business Logic Interfaces

Service interfaces currently live in `src/FulboUY.Application/Interfaces`.

Implemented interfaces:

- `IAuthService`
- `IPlayerProfileService`
- `IMatchService`
- `ITeamBalancingService`
- `ICostSplitService`

Missing for Product CRUD / US15:

- No `IProductService`.

### Business Logic

Implemented services:

- `AuthService`
- `PlayerProfileService`
- `MatchService`
- `TeamBalancingService`
- `CostSplitService`

Missing for Product CRUD / US15:

- No `ProductService`.

### Business Logic Tests

Existing test project:

- `tests/FulboUY.Application.Tests`

Existing test files:

- `AuthServiceTests.cs`
- `PlayerProfileServiceTests.cs`
- `MatchServiceTests.cs`
- `TeamBalancingServiceTests.cs`
- `CostSplitServiceTests.cs`
- `TestData.cs`

Missing for Product CRUD / US15:

- No `ProductServiceTests.cs`.

### Data Access

Implemented EF Core context:

- `AppDbContext`

Implemented DbSets:

- `Users`
- `PlayerProfiles`
- `Matches`
- `MatchParticipants`
- `InviteLinks`

Implemented repositories:

- `UserRepository`
- `PlayerProfileRepository`
- `MatchRepository`
- `MatchParticipantRepository`
- `InviteLinkRepository`

Missing for Product CRUD / US15:

- No `DbSet<Product>`.
- No Product EF configuration.
- No `ProductRepository`.
- No Product migration/table.

### Web API

Implemented controllers:

- `AuthController`
- `PlayerProfileController`
- `MatchController`

Implemented DTO groups:

- `Auth`
- `PlayerProfile`
- `Match`

Implemented DI registrations:

- Auth service/repository.
- Player profile service/repository.
- Match service/repository.
- Match participant repository.
- Invite link repository.
- Team balancing service.
- Cost split service.

Missing for Product CRUD / US15:

- No `ProductController`.
- No Product request DTOs.
- No Product response DTOs.
- No Product service/repository DI registrations.

## Test Status

Command used:

```powershell
dotnet test FulboUY.sln
```

Result:

- Passed: 19
- Failed: 0
- Skipped: 0
- Total: 19

## Recently Changed Files

There are no currently modified or added local files other than this status document.

Recent committed changes:

### `cbde2ec` - `test: add application service unit tests`

- Added `AGENTS.md`.
- Modified `FulboUY.sln`.
- Added `tests/FulboUY.Application.Tests/FulboUY.Application.Tests.csproj`.
- Added unit tests for:
  - Auth service.
  - Cost split service.
  - Match service.
  - Player profile service.
  - Team balancing service.
- Added shared test data helper.

### `882cd14` - `fix: corregir bugs críticos y agregar seguridad básica en FASE 1`

- Modified auth and match controllers.
- Modified auth and match DTOs.
- Added global exception middleware.
- Modified API startup configuration.
- Modified auth and match services.
- Modified frontend login and match detail pages.
- Added/moved `.claude` documentation and local skill files.

## Product CRUD / US15 Gap

Product CRUD / US15 still needs a complete vertical implementation:

- Product domain entity.
- Product repository interface.
- Product service interface.
- Product service implementation.
- Product service unit tests.
- Product EF DbSet and configuration.
- Product repository implementation.
- Product migration.
- Product API DTOs.
- Product controller.
- Product DI registrations.
- End-to-end API verification.

## Safest Next Steps

1. Create a feature branch from `main`.

   Suggested branch:

   ```powershell
   git checkout -b feature/us15-product-crud
   ```

2. Add failing Product business logic tests first.

   Start with `ProductServiceTests.cs` covering:

   - Create product.
   - Get product by ID.
   - List products.
   - Update product.
   - Delete product.
   - Required validation rules from US15.

3. Implement Product CRUD vertically and minimally.

   Recommended order:

   - Domain entity.
   - Application DTOs/interfaces.
   - Business logic service.
   - EF DbSet/configuration.
   - Repository.
   - API DTOs/controller.
   - DI registrations.
   - EF migration.
   - `dotnet test FulboUY.sln`.

