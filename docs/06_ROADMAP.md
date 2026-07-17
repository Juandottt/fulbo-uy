# Roadmap

## Completed

1. Stabilized SDK selection with .NET SDK `8.0.422`.
2. Implemented US15 Product CRUD as a backend vertical slice.
3. Added Product service unit tests and EF migration `20260717074704_AddProducts`.
4. Updated project documentation and handoff to match the repository.

## Next

1. Commit the documentation update.
2. Merge `feat/us15-product-crud` or prepare its pull request.
3. Start `feature/admin-management` as a backend-only slice using TDD.

Admin Management should include user listing, user detail, validated role updates, Admin-only authorization, and a last-admin guard.

## Later

1. Complete Invite Link creation, consumption, and expiration rules.
2. Add API/integration tests for routing, authorization, and EF persistence.
3. Add a CI pipeline for restore, build, and tests.
4. Repair and verify Docker/Compose deployment.
5. Add Product frontend functionality only when explicitly prioritized.
