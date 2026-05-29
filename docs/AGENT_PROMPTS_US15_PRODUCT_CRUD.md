# Agent Prompts — FulboUY US15 Product CRUD

## 1. Organizer Prompt

Use this first in Antigravity / Claude Code / Codex.

```text
Read the repository and organize the next work for Product CRUD / US15.

Important:
- Do not modify files yet.
- Inspect the current branch, solution structure, existing services, repositories, controllers, DTOs, and tests.
- Confirm whether we are on main or a feature branch.
- Confirm current test status if possible.
- Then produce:
  1. A layer-by-layer checklist for Product CRUD.
  2. The safest branch name.
  3. The first 3 commits we should aim for.
  4. The exact files that probably need to be created or modified.
  5. Any assumptions you need me to approve before coding.

Do not implement anything yet.
```

---

## 2. Branch Creator Prompt

Use this when you want the agent to create the branch only.

```text
Create a new feature branch for Product CRUD / US15.

Rules:
- Start from main.
- Check git status first.
- If the working tree is not clean, stop and explain.
- If main is clean, create this branch:
  feature/us15-product-crud
- Do not modify files.
- Do not commit.
- After creating the branch, show:
  - current branch
  - git status
  - latest 3 commits
```

Manual commands:

```powershell
git checkout main
git pull
git status
git checkout -b feature/us15-product-crud
git status
```

---

## 3. PDR Creator Prompt

Use this to make/update a project planning doc.

```text
Create a PDR document for US15 Product CRUD.

Use the current repository architecture and existing code style.

The PDR must include:
- Problem
- Goal
- Scope
- Out of scope
- Proposed Product model
- Business rules
- API endpoints
- DTOs
- Test plan
- Implementation checklist
- Risks
- Definition of done

Do not implement code.
Create or update docs/PDR_US15_PRODUCT_CRUD.md only.
```

---

## 4. Tests-First Prompt

Use this after the branch exists.

```text
Start Product CRUD / US15 using TDD.

Rules:
- Work only on Product service tests first.
- Inspect existing application tests and copy the project style.
- Create ProductServiceTests.cs.
- Add failing tests for:
  - create product success
  - create product invalid name
  - create product invalid price
  - get product by id
  - list products
  - update product
  - delete product
- Do not implement ProductService yet unless needed to compile minimal interfaces.
- Keep changes minimal.
- Run dotnet test and show the failing/compile status.
```

---

## 5. Vertical Implementation Prompt

Use this after tests are created.

```text
Implement Product CRUD / US15 vertically and minimally.

Follow this order:
1. Product domain entity
2. IProductRepository
3. IProductService
4. ProductService
5. Make ProductServiceTests pass
6. Product DbSet/configuration in AppDbContext
7. ProductRepository
8. Product DTOs
9. ProductController
10. DI registrations
11. EF migration if appropriate

Rules:
- Keep controllers thin.
- Business rules go in ProductService.
- DataAccess only handles persistence.
- Use DTOs in WebApi.
- Do not refactor unrelated features.
- Run dotnet test FulboUY.sln after meaningful changes.
- Stop and summarize before any git commit.
```

---

## 6. Commit Review Prompt

Use this before committing.

```text
Review the current changes before committing.

Run or inspect:
- git status
- git diff
- dotnet test FulboUY.sln

Then tell me:
1. Files changed.
2. Whether tests pass.
3. Whether the changes match Product CRUD / US15 only.
4. Suggested Conventional Commit message.
5. Any risk before committing.

Do not commit yet.
```

---

## 7. PR Summary Prompt

Use this when the feature is done.

```text
Prepare a PR summary for feature/us15-product-crud.

Include:
- What was implemented
- Main files changed
- Tests added/updated
- How to test manually
- Risks/assumptions
- Checklist before merge

Do not merge or push unless I explicitly ask.
```
