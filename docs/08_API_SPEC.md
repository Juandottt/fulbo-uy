# API Specification

All protected endpoints require a Bearer JWT. Admin endpoints require the `Admin` role.

## Authentication

| Method | Route | Authorization | Purpose |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Register and return JWT |
| POST | `/api/auth/login` | Public | Authenticate and return JWT |

## Player Profiles

| Method | Route | Authorization | Purpose |
|---|---|---|---|
| POST | `/api/players` | Authenticated | Create own profile |
| GET | `/api/players/me` | Authenticated | Get own profile |
| GET | `/api/players/{id}` | Authenticated | Get profile by ID |
| PUT | `/api/players/{id}` | Authenticated owner | Update profile |

## Matches, Participants, Teams, and Payments

| Method | Route | Authorization | Purpose |
|---|---|---|---|
| POST | `/api/matches` | Admin | Create match |
| GET | `/api/matches` | Authenticated | List matches |
| GET | `/api/matches/{id}` | Authenticated | Get match |
| POST | `/api/matches/{id}/join` | Authenticated | Join match |
| GET | `/api/matches/{id}/participants` | Authenticated | List participants |
| POST | `/api/matches/{id}/balance-teams` | Admin | Assign balanced teams |
| GET | `/api/matches/{id}/teams` | Authenticated | Get assigned teams |
| GET | `/api/matches/{id}/cost-split` | Authenticated | Calculate cost split |
| GET | `/api/matches/{id}/payment-status` | Authenticated | Get payment status |
| POST | `/api/matches/{id}/participants/{participantId}/pay` | Admin | Confirm payment |

## Products / US15

| Method | Route | Authorization | Purpose |
|---|---|---|---|
| POST | `/api/products` | Admin | Create product |
| GET | `/api/products` | Authenticated | List active products |
| GET | `/api/products/{id}` | Authenticated | Get product by ID |
| PUT | `/api/products/{id}` | Admin | Update product |
| DELETE | `/api/products/{id}` | Admin | Soft-delete product |

Product requests accept `name`, optional `description`, and `price`. Responses expose `id`, `name`, `description`, `price`, `isActive`, and `createdAt`; persistence entities are not returned directly.

Current caveat: direct ID lookup does not filter inactive products. There are no Product API/integration tests yet.

## Not Available

- No Admin user-management endpoints exist.
- No Invite Link endpoints exist.
