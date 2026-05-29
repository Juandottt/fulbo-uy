# API Spec

This document reflects endpoints currently found in the ASP.NET Core controllers.

## Authentication

| Method | Route | Purpose | Auth | Request DTO | Response DTO | Status Codes |
|---|---|---|---|---|---|---|
| POST | `/api/auth/register` | Register a new user | No | `RegisterRequest` | `AuthResponse` | `201`, `409` |
| POST | `/api/auth/login` | Log in and receive JWT | No | `LoginRequest` | `AuthResponse` | `200`, `401` |

## Player Profiles

Base route: `/api/players`

| Method | Route | Purpose | Auth | Request DTO | Response DTO | Status Codes |
|---|---|---|---|---|---|---|
| POST | `/api/players` | Create profile for authenticated user | JWT | `CreatePlayerProfileRequest` | `PlayerProfileResponse` | `201`, `400`, `409` |
| GET | `/api/players/{id}` | Get profile by ID | JWT | None | `PlayerProfileResponse` | `200`, `404` |
| GET | `/api/players/me` | Get authenticated user's profile | JWT | None | `PlayerProfileResponse` | `200`, `404` |
| PUT | `/api/players/{id}` | Update profile | JWT, owner enforced in service | `UpdatePlayerProfileRequest` | `PlayerProfileResponse` | `200`, `400`, `403`, `404` |

## Matches

Base route: `/api/matches`

| Method | Route | Purpose | Auth | Request DTO | Response DTO | Status Codes |
|---|---|---|---|---|---|---|
| POST | `/api/matches` | Create a match | JWT, Admin | `CreateMatchRequest` | `MatchResponse` | `201`, `400`, `403` |
| GET | `/api/matches` | List all matches | JWT | None | `IEnumerable<MatchResponse>` | `200` |
| GET | `/api/matches/{id}` | Get match by ID | JWT | None | `MatchResponse` | `200`, `404` |
| POST | `/api/matches/{id}/join` | Join a match | JWT | None | `MatchParticipantResponse` | `201`, `400`, `404`, `409` |
| GET | `/api/matches/{id}/participants` | List match participants | JWT | None | `IEnumerable<MatchParticipantResponse>` | `200`, `404` |
| POST | `/api/matches/{id}/balance-teams` | Balance teams | JWT, Admin | None | `TeamsResultResponse` | `200`, `400`, `404` |
| GET | `/api/matches/{id}/teams` | View assigned teams | JWT | None | `TeamsResultResponse` | `200`, `404` |
| GET | `/api/matches/{id}/cost-split` | View cost split | JWT | None | `CostSplitResponse` | `200`, `400`, `404` |
| POST | `/api/matches/{id}/participants/{participantId}/pay` | Confirm participant payment | JWT, Admin | None | `MatchParticipantResponse` | `200`, `400`, `404`, `409` |
| GET | `/api/matches/{id}/payment-status` | View payment status | JWT | None | `IEnumerable<PaymentStatusResponse>` | `200`, `404` |

## Product CRUD / US15

Status: Planned.

Endpoints: To be defined.

## Notes

- JWT token format and claim details are implemented in `AuthService`.
- Request validation attributes are To be defined.
- API versioning is To be defined.
- Pagination, filtering, and sorting are To be defined.
