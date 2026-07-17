# FulboUY Handoff

## Current Branch and Commits

- Branch: `feat/us15-product-crud`
- `5e36324 chore: pin dotnet sdk version`
- `ca975af feat(product): implement product crud`

## Completed Work

- Pinned .NET SDK to `8.0.422`.
- Implemented Product CRUD / US15 as a complete backend vertical slice.
- Added Domain entity, Application DTO/contracts/service, EF repository and `DbSet`, API DTOs/controller, DI, and migration `20260717074704_AddProducts`.
- Added 13 Product test methods / 17 test cases.
- Verified backend build and 36 passing tests.
- Previously verified the frontend build during stabilization; US15 did not change frontend files.

## Product API

- `POST /api/products` — Admin
- `GET /api/products` — authenticated, active products only
- `GET /api/products/{id}` — authenticated
- `PUT /api/products/{id}` — Admin
- `DELETE /api/products/{id}` — Admin, soft delete

## Known Risks and Gaps

- Product has no API/integration tests for routing, authorization, or EF persistence.
- Direct Product lookup can return inactive products; desired non-admin behavior needs a decision.
- Admin Management has not started.
- Invite Links are only partially implemented in Domain/Infrastructure.
- Docker/Compose has known build blockers and was not changed.
- NuGet vulnerability lookup may emit `NU1900` when the service is unavailable.
- `.agents/` is untracked and must not be staged.

## Recommended Sequence

1. Review and commit this documentation update.
2. Merge or prepare a PR for `feat/us15-product-crud`.
3. Create `feature/admin-management` from the agreed integration branch.
4. Later complete Invite Link Expiration, integration tests, CI, and Docker/deployment.

Do not mix Admin Management implementation into the US15 branch.
