# FulboUY Project Overview

## Purpose

FulboUY is a .NET 8 and React application for organizing amateur football matches. It supports player registration, match participation, team balancing, cost sharing, payment confirmation, and an administrator-managed product catalog.

## Architecture

The backend follows a four-layer Clean Architecture:

- `FulboUY.Domain`: entities and domain types without infrastructure dependencies.
- `FulboUY.Application`: DTOs, service contracts, repository contracts, and business rules.
- `FulboUY.Infrastructure`: Entity Framework Core, SQL Server persistence, repositories, and migrations.
- `FulboUY.API`: HTTP controllers, API request/response DTOs, authentication, authorization, and dependency injection.

The React client is under `client/`. Backend unit tests are under `tests/FulboUY.Application.Tests/`.

## Current State

Implemented backend features:

- JWT authentication and Admin/Player roles
- Player profiles
- Matches and participants
- Team balancing
- Cost split and payment confirmation
- Product CRUD / US15

Product CRUD is backend-only. Admin Management has not started. Invite Links currently have a Domain entity, repository, `DbSet`, migration, and DI registration, but no Application service or API endpoints.

## Verification Baseline

- SDK pinned to .NET `8.0.422` by `global.json`.
- Backend build passes.
- Backend tests: 36 passed, 0 failed, 0 skipped.
- Frontend build was verified during project stabilization; US15 made no frontend changes.
- Docker configuration has known blockers and was not changed by US15.

See [API Spec](08_API_SPEC.md), [Test Strategy](09_TEST_STRATEGY.md), and the root [HANDOFF](../HANDOFF.md) for operational details.
