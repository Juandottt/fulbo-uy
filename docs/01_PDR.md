# Product Requirements Document

## Problem

Organizing amateur football matches requires coordinating players, costs, availability, teams, and payments. Without a central tool, organizers handle this manually through messages and spreadsheets.

## Goal

Provide a simple web application that supports match organization from registration through team balancing and cost tracking.

## Scope

In scope:

- Authentication.
- Player profiles.
- Match creation and listing.
- Match joining.
- Participant management.
- Team balancing.
- Cost split calculation.
- Payment status tracking.
- Product CRUD / US15 as a planned feature.

## Out Of Scope

- Real payment processing.
- Mobile native apps.
- Public production hosting details.
- Advanced scheduling.
- Chat/messaging.
- Push notifications.

## Functional Requirements

- Users can register and log in.
- Authenticated users can manage their player profile.
- Admin users can create matches.
- Authenticated users can view matches.
- Authenticated users can join open matches.
- The system prevents duplicate match participation.
- The system marks a match as full when capacity is reached.
- Admin users can balance teams.
- Users can view cost split information.
- Admin users can confirm participant payments.
- Product CRUD / US15 requirements are To be defined.

## Non-Functional Requirements

- Backend should remain layered and testable.
- Controllers should be thin.
- Business rules should be unit tested.
- Secrets must not be committed.
- API responses should use DTOs.
- Database access should go through repositories.

## Assumptions

- SQL Server is the target database.
- JWT is the authentication mechanism.
- Roles include at least Admin and Player.
- Product CRUD / US15 belongs in the same backend architecture.

## Risks

- Product CRUD / US15 acceptance criteria are not present in the current codebase.
- Current README and docs may lag behind implementation unless maintained.
- No integration tests currently verify API/database behavior.
- Frontend feature coverage is incomplete.
