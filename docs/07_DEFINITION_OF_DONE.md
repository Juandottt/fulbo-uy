# Definition Of Done

## Feature Done Checklist

- Requirement is clear or marked as To be defined.
- Implementation follows existing architecture.
- Business rules are in Application services.
- API uses request/response DTOs.
- Error handling and status codes are appropriate.
- No unrelated refactors.
- No secrets committed.

## Backend Done Checklist

- Domain model updated when needed.
- Application interfaces and services updated.
- Infrastructure repository/DbContext updated when needed.
- API controller and DTOs added or updated.
- DI registrations added.
- EF migration added when schema changes.
- `dotnet build FulboUY.sln` passes.
- `dotnet test FulboUY.sln` passes.

## Frontend Done Checklist

- UI follows existing React/Vite structure.
- API calls are placed in `client/src/api` when practical.
- Authenticated flows respect existing auth context.
- Loading and error states are handled.
- `npm run build` passes if available.

## Testing Done Checklist

- Important business rules have unit tests.
- Edge cases are covered where practical.
- Existing tests are not removed or weakened.
- Manual API testing performed when endpoints change.
- Integration tests added for high-risk persistence/API flows when practical.

## Documentation Done Checklist

- README updated if setup or status changed.
- API spec updated if endpoints changed.
- User stories or backlog updated if scope changed.
- Decision log updated for meaningful architecture decisions.

## PR Ready Checklist

- Branch is up to date with the intended base.
- `git status` reviewed.
- Diff contains only intended changes.
- Tests/builds run and results noted.
- PR description explains what changed and why.
- No generated artifacts committed unless intentionally required.
