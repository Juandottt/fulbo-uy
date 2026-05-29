# Git Workflow

## Branch Model

Use short-lived branches from the appropriate base branch.

## `main`

Purpose:

- Stable branch.
- Should contain reviewed, working code.
- Do not commit directly.

## `develop`

Purpose:

- Integration branch for upcoming work.
- Current repository may not have this branch locally.
- To be defined if the team wants Git Flow-style development.

## Branch Naming

Feature branches:

```txt
feature/us15-product-crud
feature/match-payment-ui
```

Fix branches:

```txt
fix/auth-token-claims
fix/match-capacity-check
```

Documentation branches:

```txt
docs/project-documentation
docs/api-spec
```

Test branches:

```txt
test/product-service
```

## Commit Message Convention

Use Conventional Commits:

```txt
docs: update project documentation
test(product): add product service tests
feat(product): implement product crud
fix(match): prevent duplicate participants
refactor(auth): simplify token creation
chore: update docker configuration
```

## Pull Request Checklist

- Branch name follows the convention.
- Scope is focused.
- Source changes are covered by tests where practical.
- `dotnet test FulboUY.sln` passes for backend changes.
- Frontend build/test command passes for frontend changes if available.
- Documentation updated when behavior changes.
- No secrets committed.
- No unrelated formatting or large refactors.
- Reviewer can understand the change from the PR description.
