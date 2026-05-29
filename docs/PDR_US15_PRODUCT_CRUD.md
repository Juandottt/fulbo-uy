# PDR — US15 Product CRUD

## Feature Name

US15 — Product CRUD

## Project

FulboUY

## Date

2026-05-29

---

## Problem

The current FulboUY backend has authentication, player profiles, matches, participants, team balancing, cost split, and payment confirmation foundations.

However, Product CRUD / US15 does not exist yet.

Missing pieces include:

- Product domain entity
- Product repository interface
- Product service interface
- Product service implementation
- Product service tests
- Product EF DbSet/configuration
- Product repository
- Product migration/table
- Product API DTOs
- Product controller
- Product DI registrations
- End-to-end API verification

---

## Goal

Implement a minimal, clean, testable Product CRUD feature following the existing architecture.

The implementation should respect:

- Clean Architecture
- Thin controllers
- Business logic in Application/BusinessLogic layer
- Persistence logic in DataAccess
- DTOs for API input/output
- MSTest + Moq for service tests
- Small commits

---

## Scope

### In Scope

Product CRUD backend:

- Create product
- Get product by ID
- List products
- Update product
- Delete product or mark product inactive

Required technical work:

- Domain model
- Repository interface
- Service interface
- Service implementation
- Unit tests
- EF Core DbSet/configuration
- Repository implementation
- Controller
- DTOs
- DI registration
- Migration
- Manual API testing

### Out of Scope

Do not include yet:

- React frontend
- Product images upload
- Payment integration
- Advanced search
- Admin roles redesign
- Large architecture refactor
- Match feature changes

---

## Proposed Product Model

Start minimal.

```csharp
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
```

Optional later:

- Category
- Stock
- ImageUrl
- UpdatedAt
- Owner/Admin relation

---

## Business Rules

Minimum recommended rules:

- Name is required.
- Name cannot be only whitespace.
- Description is optional or required depending on project need.
- Price must be greater than zero.
- CreatedAt is set when creating the product.
- Product must exist before update/delete.
- Deleted products can either:
  - be physically deleted, or
  - marked inactive with `IsActive = false`.

Recommended for first version:

```txt
Use soft delete with IsActive = false.
```

Why:

- Safer for future match/payment/order history.
- Avoids accidental data loss.
- Easy to filter active products.

---

## API Endpoints

Recommended endpoints:

```http
POST   /api/products
GET    /api/products
GET    /api/products/{id}
PUT    /api/products/{id}
DELETE /api/products/{id}
```

Expected responses:

| Endpoint | Success | Common errors |
|---|---:|---|
| POST `/api/products` | 201 Created | 400 |
| GET `/api/products` | 200 OK | - |
| GET `/api/products/{id}` | 200 OK | 404 |
| PUT `/api/products/{id}` | 204 No Content or 200 OK | 400 / 404 |
| DELETE `/api/products/{id}` | 204 No Content | 404 |

---

## DTOs

Suggested DTOs:

```txt
CreateProductRequest
UpdateProductRequest
ProductResponse
```

Example:

```csharp
public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

---

## Test Plan

Start with service tests.

Create:

```txt
tests/FulboUY.Application.Tests/ProductServiceTests.cs
```

Minimum tests:

1. Create product succeeds with valid data.
2. Create product fails when name is empty.
3. Create product fails when price is zero or negative.
4. Get product by ID returns product when found.
5. Get product by ID fails/returns null when missing, depending on existing style.
6. List products returns products.
7. Update product succeeds when product exists.
8. Update product fails when product does not exist.
9. Delete product marks product inactive or deletes it.

---

## Implementation Checklist

### Phase 1 — Branch

```powershell
git checkout main
git pull
git checkout -b feature/us15-product-crud
```

### Phase 2 — Tests First

- Add `ProductServiceTests.cs`
- Mock `IProductRepository`
- Write failing tests for main use cases

### Phase 3 — Domain/Application

- Add `Product` entity
- Add `IProductRepository`
- Add `IProductService`
- Add `ProductService`

### Phase 4 — DataAccess

- Add `DbSet<Product>` to `AppDbContext`
- Add Product EF configuration if needed
- Add `ProductRepository`
- Add migration

### Phase 5 — WebApi

- Add Product DTOs
- Add `ProductController`
- Register service/repository in DI
- Verify endpoints manually

### Phase 6 — Final Verification

```powershell
dotnet build
dotnet test FulboUY.sln
```

Manual test:

- Create product
- List products
- Get product by ID
- Update product
- Delete product
- Confirm deleted/inactive behavior

---

## Risks

| Risk | Mitigation |
|---|---|
| Product rules unclear | Start minimal and document assumptions |
| Feature gets too big | Backend only first |
| Breaking existing API | Run all 19 existing tests after each phase |
| Agent over-refactors | Use AGENTS.md and small layer-by-layer prompts |
| Migration issues | Keep Product table independent first |

---

## Final Deliverable

US15 is complete when:

- Product backend CRUD works end-to-end
- Tests pass
- API endpoints work
- Code follows existing architecture
- Branch is ready for PR
