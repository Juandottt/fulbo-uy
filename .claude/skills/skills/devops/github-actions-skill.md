# GitHub Actions Skill

Goal:
Automate build and test of the .NET 8 solution using GitHub Actions.

Context:
Project follows layered architecture:
WebApi → BusinessLogic → DataAccess → Domain

Rules:

- Workflow must run on every Pull Request to develop or main
- Build must fail if tests fail
- Use .NET 8 SDK
- Restore dependencies before build
- Run all tests in solution
- Do not deploy automatically

Standard workflow structure:

name: CI

on:
pull_request:
branches: - develop - main

jobs:
build:
runs-on: ubuntu-latest

    steps:
      - name: Checkout repo
        uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore --configuration Release

      - name: Run tests
        run: dotnet test --no-build --configuration Release

Best practices:

- Keep workflow simple
- No secrets required
- Fail fast if any test fails
- Use pull request as quality gate
