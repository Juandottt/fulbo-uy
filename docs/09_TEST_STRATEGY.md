# Test Strategy

## Current Test Projects

- `tests/FulboUY.Application.Tests`

## Current Testing Framework

- MSTest
- Moq
- FluentAssertions
- coverlet collector

## Current Test Coverage Focus

Existing tests cover Application services:

- `AuthService`
- `PlayerProfileService`
- `MatchService`
- `TeamBalancingService`
- `CostSplitService`

Current baseline:

- Passed: 19
- Failed: 0
- Skipped: 0

## What Should Be Unit Tested

- Business rules in Application services.
- Domain validation if domain behavior is added.
- Edge cases for match joining.
- Authorization-sensitive service behavior where applicable.
- Product CRUD / US15 business rules when implemented.

## What Should Be Integration Tested

- Controller behavior and status codes.
- EF Core repository behavior.
- Database constraints.
- Authentication and authorization flows.
- Product CRUD API and persistence once implemented.

## Manual API Testing Strategy

Use Swagger or an API client to verify:

- Register user.
- Login and copy JWT.
- Create player profile.
- Create match as Admin.
- List matches.
- Join match as Player.
- View participants.
- Balance teams as Admin.
- View cost split.
- Confirm payment as Admin.

Manual setup for Admin users is To be defined.

## Commands

Run all backend tests:

```powershell
dotnet test FulboUY.sln
```

Build backend:

```powershell
dotnet build FulboUY.sln
```

Frontend build:

```powershell
cd client
npm run build
```

Frontend test command: To be defined.
