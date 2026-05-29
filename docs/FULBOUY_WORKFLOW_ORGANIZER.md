# FulboUY — Workflow Organizer

## Current Status

Date: 2026-05-29

Current repository state:

- Current branch: `main`
- Working tree: clean
- Tests: `19 passed / 0 failed / 0 skipped`
- Backend foundation exists:
  - JWT authentication
  - Player profile creation, lookup, and update
  - Match creation and listing
  - Match joining
  - Match participant listing
  - Team balancing
  - Cost split calculation
  - Payment confirmation at API/service level

Product CRUD / US15 is not implemented yet.

---

## Goal

Create a safe workflow for implementing Product CRUD / US15 without breaking the current stable backend.

Main idea:

```txt
main stays stable
feature/us15-product-crud contains all Product CRUD work
small commits by layer
tests after each meaningful step
```

---

## Branch Strategy

### Main Branch

`main`

Use for:

- Stable code only
- Already tested backend foundation
- Merge completed features only through PR

Do not work directly on `main`.

### Feature Branch

Create:

```powershell
git checkout main
git pull
git checkout -b feature/us15-product-crud
```

Use for:

- Product domain entity
- Product service
- Product repository
- Product controller
- Product DTOs
- Product tests
- Product migration

---

## Safe Daily Startup Checklist

Run this before coding:

```powershell
git branch
git status
dotnet test FulboUY.sln
```

Expected before starting:

```txt
On branch feature/us15-product-crud
working tree clean
tests passing
```

If you are on `main`, switch/create the feature branch first.

---

## Safe Daily Shutdown Checklist

Run this before stopping:

```powershell
dotnet test FulboUY.sln
git status
git diff
```

If tests pass and changes are good:

```powershell
git add .
git commit -m "feat(product): implement product layer checkpoint"
git push origin feature/us15-product-crud
```

Only commit after reviewing the diff.

---

## Recommended Commit Plan

### Commit 1 — Product domain + tests

```powershell
git commit -m "test(product): add product service tests"
```

or, if domain/tests are included:

```powershell
git commit -m "feat(product): add product domain model"
```

### Commit 2 — Application interfaces + ProductService

```powershell
git commit -m "feat(product): implement product service"
```

### Commit 3 — DataAccess repository + DbContext

```powershell
git commit -m "feat(product): add product repository"
```

### Commit 4 — WebApi DTOs + controller + DI

```powershell
git commit -m "feat(product): expose product API endpoints"
```

### Commit 5 — Migration / database

```powershell
git commit -m "feat(product): add product migration"
```

### Commit 6 — Cleanup

```powershell
git commit -m "refactor(product): clean up product crud implementation"
```

---

## Implementation Order

Follow this order to avoid chaos:

1. `ProductServiceTests.cs`
2. `Product` domain entity
3. `IProductRepository`
4. `IProductService`
5. `ProductService`
6. Product service tests passing
7. `DbSet<Product>` in `AppDbContext`
8. Product EF configuration if needed
9. `ProductRepository`
10. Product DTOs
11. `ProductController`
12. DI registrations
13. EF migration
14. Manual API verification
15. Final `dotnet test FulboUY.sln`

---

## Do Not Do Yet

Avoid these until Product CRUD backend is stable:

- Frontend screens
- Large refactors
- Authentication redesign
- Changing existing match/profile behavior
- Moving folders around
- Renaming existing services/controllers
- Changing database relationships unrelated to Product

---

## Antigravity / Agent Rule

For agents:

```txt
Do not implement everything at once.
Work one layer at a time.
After each layer, stop and summarize:
- files changed
- tests run
- what remains
```

---

## Definition of Done

Product CRUD / US15 is done when:

- Product can be created
- Product can be listed
- Product can be fetched by ID
- Product can be updated
- Product can be deleted or deactivated, depending on chosen business rule
- Product service has unit tests
- Repository is registered
- Service is registered
- Controller is available
- API returns DTOs
- `dotnet test FulboUY.sln` passes
- Manual API verification works
