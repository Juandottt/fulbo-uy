# User Stories

## Current Feature Status

| Area | Status | Notes |
|---|---|---|
| Authentication | Done | Register/login with JWT |
| Player Profiles | Done | Create, read, and update |
| Matches | Partial | Create, list, get, and join; no update/delete lifecycle |
| Participants | Partial | Join and list; no leave/remove flow |
| Team Balancing | Done | Admin-triggered backend flow |
| Cost Split | Done | Per-player calculation and payment totals |
| Payment Confirmation | Done | Admin backend endpoint; UI exists |
| Product CRUD / US15 | Done, backend only | Complete backend vertical slice |
| Invite Links | Partial | Domain/Infrastructure only |
| Admin Management | Not started | Planned next backend feature |

## US15 — Product CRUD

As an Admin, I can manage products so authenticated users can browse the active catalog.

Implemented behavior:

- Admin can create a product.
- Authenticated users can list active products.
- Authenticated users can get a product by ID.
- Admin can update a product.
- Admin can soft-delete a product by setting `IsActive` to `false`.

Product fields:

- `Id: Guid`
- `Name: string`, required and not whitespace
- `Description: string`, empty when omitted
- `Price: decimal`, greater than zero
- `IsActive: bool`, true by default
- `CreatedAt: DateTime`

Acceptance status: complete for the backend. API/integration coverage and frontend Product UI are not part of the completed slice.

Open product decision: `GET /api/products/{id}` currently returns an inactive product when addressed directly. Decide whether non-admin users should receive `404` instead.
