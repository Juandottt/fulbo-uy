FulboUY - API REST para organizar partidos de fútbol
======================================================

Descripción
-----------
FulboUY es una API REST desarrollada en .NET 8 para organizar partidos de fútbol 5 o fútbol 7.

Funcionalidades
---------------
- Registro y autenticación de usuarios (JWT)
- Perfil futbolístico con atributos: rapidez, técnica, pase, defensa
- Creación y gestión de partidos
- Invitación de jugadores por link
- Confirmación de asistencia
- División del costo de la cancha entre los participantes
- Generación automática de equipos equilibrados

Tecnologías
-----------
- .NET 8
- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- JWT Authentication
- Swagger / OpenAPI

Arquitectura
------------
Arquitectura en capas:
  - Controllers   : Endpoints HTTP, manejo de requests/responses
  - Services      : Lógica de negocio
  - Repositories  : Acceso a datos (patrón Repository)
  - Entities      : Modelos de dominio / tablas de base de datos
  - DTOs          : Objetos de transferencia de datos

Estructura del proyecto
-----------------------
fulbouy/
  src/
    FulboUY.API/
      Controllers/
      DTOs/
    FulboUY.Application/
      Services/
      Interfaces/
    FulboUY.Domain/
      Entities/
    FulboUY.Infrastructure/
      Repositories/
      Data/
  .claude/
    skills/
  README.txt
  CLAUDE.md

Inicio rápido
-------------
1. Clonar el repositorio
2. Configurar la cadena de conexión en appsettings.json
3. Ejecutar migraciones: dotnet ef database update
4. Iniciar la API: dotnet run
5. Acceder a Swagger: https://localhost:{puerto}/swagger
