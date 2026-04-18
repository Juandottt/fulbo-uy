# FulboUY — Contexto para Claude Code

## Descripción del proyecto

API REST en **.NET 8** para organizar partidos de fútbol 5 o fútbol 7.
Permite registrar usuarios, crear partidos, invitar jugadores, confirmar asistencia,
dividir el costo de la cancha y generar equipos equilibrados automáticamente.

## Stack tecnológico

- **Runtime:** .NET 8
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Base de datos:** SQL Server
- **Autenticación:** JWT (Bearer tokens)
- **Documentación:** Swagger / OpenAPI

## Arquitectura: Capas

```
FulboUY.API/             → Controllers, DTOs, configuración de la API
FulboUY.Application/     → Services, Interfaces (lógica de negocio)
FulboUY.Domain/          → Entities (modelos de dominio)
FulboUY.Infrastructure/  → Repositories, DbContext, migraciones EF Core
```

## Convenciones de código

- Idioma del código: **inglés** (nombres de clases, métodos, variables)
- Idioma de comentarios y commits: **español**
- Inyección de dependencias en todos los servicios y repositorios
- Repositorios con interfaz (IUserRepository → UserRepository)
- Servicios con interfaz (IMatchService → MatchService)
- DTOs separados de las Entities en todo momento
- Respuestas HTTP estandarizadas con códigos correctos (200, 201, 400, 401, 404, 409)

## Entidades principales

- **User** — usuario registrado con perfil futbolístico
- **PlayerProfile** — atributos: velocidad, técnica, pase, defensa (1–10)
- **Match** — partido con fecha, lugar, modalidad (5 o 7), costo de cancha
- **MatchParticipant** — relación usuario ↔ partido con estado de confirmación
- **InviteLink** — token único para invitar jugadores a un partido

## Reglas de negocio clave

- Los equipos se generan equilibrando la suma de atributos de los jugadores confirmados
- El costo de la cancha se divide entre los jugadores confirmados
- Un partido requiere mínimo 10 jugadores (fútbol 5) o 14 (fútbol 7) para generarse
- Los invites tienen expiración configurable

## Lo que NO se debe hacer

- No exponer las Entities directamente en los endpoints (usar DTOs)
- No poner lógica de negocio en los Controllers
- No acceder a la base de datos directamente desde Services (usar Repositories)
- No guardar passwords en texto plano (usar bcrypt o similar)

## Comandos útiles

```bash
# Crear migración
dotnet ef migrations add NombreMigracion --project FulboUY.Infrastructure --startup-project FulboUY.API

# Aplicar migraciones
dotnet ef database update --project FulboUY.Infrastructure --startup-project FulboUY.API

# Ejecutar la API
dotnet run --project src/FulboUY.API

# Ejecutar tests
dotnet test
```
