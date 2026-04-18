# FulboUY

**Estado:** 35% en desarrollo — MVP Backend completo, Frontend en progreso

API REST + interfaz React para organizar partidos de fútbol amateur en Uruguay.
Permite registrar jugadores, crear partidos, dividir costos y generar equipos equilibrados automáticamente.

---

## Features

| Feature | Estado |
|---------|--------|
| Autenticación JWT (registro/login) | ✅ Implementado |
| CRUD de partidos (crear, ver, unirse) | ✅ Implementado |
| Perfiles de jugador (velocidad, técnica, pase, defensa) | ✅ Implementado |
| Algoritmo de balanceo de equipos (greedy) | ✅ Implementado |
| División de costos de cancha | ✅ Implementado |
| Links de invitación a partidos | ✅ Implementado |
| Roles Admin/Jugador | ✅ Implementado |
| Frontend React (páginas principales) | 🚧 En progreso |
| Confirmación de pago desde UI | 🚧 En progreso |
| Deploy en producción | ❌ Pendiente |
| Tests unitarios backend | ❌ Pendiente |

---

## Tech Stack

| Capa | Tecnologías |
|------|-------------|
| **Backend** | C#, .NET 8, ASP.NET Core Web API |
| **Frontend** | React 18, JavaScript, TailwindCSS, Vite |
| **Base de datos** | SQL Server (EF Core, migraciones) |
| **Autenticación** | JWT Bearer tokens, BCrypt |
| **DevOps** | Docker, Docker Compose |
| **Documentación** | Swagger / OpenAPI |

---

## Quickstart

### Con Docker (recomendado)

```bash
git clone <repo>
cd fulbouy

# Copiar variables de entorno
cp .env.example .env
# Editar .env con tus credenciales reales

docker-compose up
```

Accesos:
- Frontend: http://localhost:5173
- API: http://localhost:5089
- Swagger: http://localhost:5089/swagger

### Manual

```bash
# Backend (.NET)
cd src/FulboUY.API
dotnet restore
dotnet run
# API disponible en http://localhost:5089

# Frontend (React)
cd client
npm install
npm run dev
# UI disponible en http://localhost:5173
```

Requisitos: .NET 8 SDK, Node.js 20+, SQL Server Express

---

## Estructura del Código

```
fulbouy/
├── src/                         # Backend .NET 8
│   ├── FulboUY.API/             # Controllers, DTOs, configuración HTTP
│   │   └── Controllers/         # AuthController, MatchController, PlayerProfileController
│   ├── FulboUY.Application/     # Servicios (lógica de negocio), interfaces
│   │   └── Services/            # AuthService, MatchService, TeamBalancingService, CostSplitService
│   ├── FulboUY.Domain/          # Entidades del dominio (sin dependencias externas)
│   │   └── Entities/            # User, Match, PlayerProfile, MatchParticipant, InviteLink
│   └── FulboUY.Infrastructure/  # Repositorios, DbContext, migraciones EF Core
├── client/                      # Frontend React + Vite
│   └── src/
│       ├── api/                 # Llamadas HTTP (axios) al backend
│       ├── context/             # AuthContext — estado global del usuario
│       ├── components/          # Navbar, PrivateRoute
│       └── pages/               # LoginPage, MatchesPage, ProfilePage, etc.
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
| POST | `/api/matches/{id}/balance` | Generar equipos | Admin |
| GET | `/api/players/me` | Mi perfil | Sí |
| PUT | `/api/players/me` | Actualizar perfil | Sí |

---

## Aprendizajes Clave

1. **Clean Architecture en .NET** — Separación estricta en 4 capas con inversión de dependencias. Permite cambiar la BD sin tocar la lógica de negocio.
2. **JWT desde cero** — Implementación manual de generación y validación de tokens. Entendí cómo funciona la autenticación stateless.
3. **Algoritmo greedy de balanceo** — Ordenar jugadores por habilidad promedio y asignar alternadamente garantiza equipos equilibrados con O(n log n).
4. **Proxy Vite para CORS** — El frontend en :5173 y backend en :5089 conviven sin errores CORS gracias al proxy de Vite.

---

## Problemas Conocidos / Pendiente

- [ ] Tests unitarios para los servicios de Application
- [ ] Página de perfil completa en el frontend
- [ ] Confirmar pagos desde la UI
- [ ] Expiración de InviteLinks no implementada aún
- [ ] Deploy a servidor (Railway / Azure)

---

## Contacto

**Juan Constantin** — Junior Full-Stack Developer | ORT Uruguay  
📧 juanconstantinb@gmail.com  
💼 Disponibilidad: Part-time 4-6 hs/día
