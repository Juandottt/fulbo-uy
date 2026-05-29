# Agent Workflow

This file describes how Codex, Claude, Antigravity, or other AI coding agents should work in this repository.

## Default Workflow

1. Inspect first.
2. Identify the relevant layer and files.
3. Summarize current behavior.
4. Plan before editing for non-trivial work.
5. Make small, focused changes.
6. Run relevant tests/builds.
7. Summarize changed files, commands run, and remaining work.

## Editing Rules

- Do not edit source code unless implementation is requested.
- Do not edit tests unless test work is requested.
- Do not edit package files without approval.
- Do not refactor large areas without approval.
- Do not delete files without approval.
- Do not commit or push without approval.

## Testing Expectations

For backend changes:

```powershell
dotnet test FulboUY.sln
```

For frontend changes:

```powershell
cd client
npm run build
```

If a command cannot be run, explain why.

## Summary Format

At the end of a task, include:

- Files changed.
- Tests or builds run.
- Assumptions made.
- Remaining work.

## Suggested Prompts

Inspect:

```txt
Read the repository and summarize the current implementation by layer. Do not modify files.
```

Implement:

```txt
Implement the smallest vertical slice for [feature]. Follow Clean Architecture, add business logic tests first, and do not commit.
```

Review:

```txt
Review the current diff for bugs, architecture violations, missing tests, and risky behavior changes.
```

Documentation:

```txt
Update only documentation files to reflect the current implementation. Do not modify source, tests, or package files.
```

## Product CRUD / US15 Agent Path

When Product CRUD / US15 is approved:

1. Confirm or document acceptance criteria.
2. Add failing Application tests.
3. Implement the smallest backend vertical slice.
4. Add EF migration only after model shape is clear.
5. Update API spec and backlog.
6. Run `dotnet test FulboUY.sln`.
