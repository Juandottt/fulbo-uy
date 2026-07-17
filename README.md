# FulboUY

**Estado:** backend MVP en desarrollo — Product CRUD / US15 completado

API REST + interfaz React para organizar partidos de fútbol amateur en Uruguay.
Permite registrar jugadores, crear partidos, dividir costos y generar equipos equilibrados automáticamente.

---

## Features

| Feature | Estado |
|---------|--------|
| Autenticación JWT (registro/login) | ✅ Implementado |
| Partidos (crear, listar, ver, unirse) | ✅ Implementado |
| Perfiles de jugador (velocidad, técnica, pase, defensa) | ✅ Implementado |
| Balanceo de equipos | ✅ Implementado |
| División de costos de cancha | ✅ Implementado |
| Confirmación de pagos | ✅ Backend y UI implementados |
| Product CRUD / US15 | ✅ Backend implementado |
| Links de invitación a partidos | 🚧 Domain/Infrastructure solamente |
| Roles Admin/Jugador | ✅ Implementado |
| Frontend React (páginas principales) | 🚧 En progreso |
| Admin Management | ❌ No iniciado |
| Deploy en producción | ❌ Pendiente |
| Tests unitarios backend | ✅ 36 tests pasando |
| Tests de integración | ❌ Pendiente |

---

## Tech Stack

| Capa | Tecnologías |
|------|-------------|
| **Backend** | C#, .NET 8, ASP.NET Core Web API |
| **Frontend** | React 19, JavaScript, TailwindCSS, Vite |
| **Base de datos** | SQL Server (EF Core, migraciones) |
| **Autenticación** | JWT Bearer tokens, BCrypt |
| **DevOps** | Docker, Docker Compose |
| **Documentación** | Swagger / OpenAPI |

---

## Quickstart

### Manual

```bash
# Backend (.NET)
cd src/FulboUY.API
dotnet restore
dotnet run
# API disponible en http://localhost:5089

# Frontend
cd client
npm ci
npm run dev
# UI disponible en http://localhost:5173
```

Requisitos: .NET 8 SDK, una versión de Node compatible con `package-lock.json`, y SQL Server. El SDK está fijado en `global.json`.

La configuración Docker/Compose tiene bloqueos conocidos y todavía no constituye un quickstart verificado.

---

## Estructura del Código

```
fulbouy/
├── src/                         # Backend .NET 8
│   ├── FulboUY.API/             # Controllers, DTOs, configuración HTTP
│   │   └── Controllers/         # Auth, Match, PlayerProfile y Product
│   ├── FulboUY.Application/     # Servicios (lógica de negocio), interfaces
│   │   └── Services/            # Casos de uso y reglas de negocio
│   ├── FulboUY.Domain/          # Entidades del dominio (sin dependencias externas)
│   │   └── Entities/            # User, Match, PlayerProfile, InviteLink, Product, etc.
│   └── FulboUY.Infrastructure/  # Repositorios, DbContext, migraciones EF Core
├── tests/                       # Tests unitarios de Application
├── client/                      # Frontend React + Vite
│   └── src/
│       ├── api/                 # Llamadas HTTP (axios) al backend
│       ├── context/             # AuthContext — estado global del usuario
│       ├── components/          # Navbar, PrivateRoute
│       └── pages/               # LoginPage, MatchesPage, ProfilePage, etc.
├── docs/                        # Estado, historias, roadmap, API y estrategia de tests
├── HANDOFF.md
├── global.json
├── FulboUY.sln
└── docker-compose.yml
```

**Arquitectura:** Clean Architecture en 4 capas (API → Application → Domain ← Infrastructure)

---

## Endpoints Principales

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Registrar usuario | No |
| POST | `/api/auth/login` | Login → JWT | No |
| GET | `/api/matches` | Listar partidos | Sí |
| POST | `/api/matches` | Crear partido | Admin |
| POST | `/api/matches/{id}/join` | Unirse a partido | Sí |
| POST | `/api/matches/{id}/balance-teams` | Generar equipos | Admin |
| GET | `/api/players/me` | Mi perfil | Sí |
| PUT | `/api/players/{id}` | Actualizar perfil propio | Sí |
| POST | `/api/products` | Crear producto | Admin |
| GET | `/api/products` | Listar productos activos | Sí |
| GET | `/api/products/{id}` | Obtener producto | Sí |
| PUT | `/api/products/{id}` | Actualizar producto | Admin |
| DELETE | `/api/products/{id}` | Desactivar producto | Admin |

---

## Aprendizajes Clave

1. **Clean Architecture en .NET** — El backend separa HTTP, casos de uso, dominio y persistencia.
2. **JWT y roles** — La API aplica autenticación stateless y autorización Admin/Jugador.
3. **TDD en Application** — Product CRUD fue desarrollado comenzando por tests de servicio.
4. **EF Core Code First** — Los cambios de persistencia se versionan mediante migraciones.

---

## Problemas Conocidos / Pendiente

- [ ] Definir si un producto inactivo debe devolver `404` por ID para usuarios no Admin
- [ ] Agregar tests API/integración para Product, autorización y EF
- [ ] Implementar Admin Management
- [ ] Completar Invite Links y su expiración
- [ ] Agregar CI
- [ ] Reparar y verificar Docker/Compose y deployment

## Documentación

- [Project overview](docs/00_PROJECT_OVERVIEW.md)
- [User stories](docs/05_USER_STORIES.md)
- [Roadmap](docs/06_ROADMAP.md)
- [API specification](docs/08_API_SPEC.md)
- [Test strategy](docs/09_TEST_STRATEGY.md)
- [Current handoff](HANDOFF.md)

---

## Contacto

**Juan Constantin** — Junior Full-Stack Developer | ORT Uruguay  
📧 juanconstantinb@gmail.com  
💼 Disponibilidad: Part-time 4-6 hs/día
