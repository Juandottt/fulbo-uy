# Test Strategy

## Current Tooling

- MSTest
- Moq
- FluentAssertions
- `Microsoft.NET.Test.Sdk`

## Current Coverage

The backend has Application service unit tests for:

- Authentication
- Player profiles
- Matches
- Team balancing
- Cost split and payment confirmation
- Product CRUD

Current result: 36 passed, 0 failed, 0 skipped.

Product coverage consists of 13 test methods and 17 executed cases covering creation, validation, null description normalization, reads, active listing, updates, and soft deletion.

## Commands

```powershell
dotnet build FulboUY.sln
dotnet test FulboUY.sln --no-restore -nr:false -m:1 /p:UseSharedCompilation=false
```

## Gaps

There are no API/integration tests. Product routing, Admin authorization, response codes, model binding, EF queries, migration application, and soft-delete persistence are not verified end to end.

Recommended next test layer:

1. Add a dedicated integration test project using `WebApplicationFactory`.
2. Verify public/authenticated/Admin authorization boundaries.
3. Verify Product active filtering and soft deletion against a test database.
4. Run build and tests in CI.
