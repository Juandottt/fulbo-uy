# Repository Pattern Skill

Goal:
Implement repository pattern using interfaces in Domain layer
and implementations in DataAccess layer.

Rules:

- Interfaces must live in Domain
- Implementations must live in DataAccess
- Logic layer depends only on interfaces
- Use Dependency Injection
- Avoid direct DbContext usage in controllers

Example:

Domain:
IUserRepository

DataAccess:
UserRepository : IUserRepository
