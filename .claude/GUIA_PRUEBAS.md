# FulboUY — Guía de Pruebas

API REST en .NET 8 para organizar partidos de fútbol amateur.

---

## Lo que se construyó

| Etapa | Qué hace |
|-------|----------|
| 1 | Estructura .NET 8: 4 proyectos, paquetes, referencias |
| 2 | Entidades del dominio: User, PlayerProfile, Match, MatchParticipant, InviteLink |
| 3 | Base de datos: AppDbContext, 5 repositorios EF Core, SQL Server |
| 4 | Auth JWT: registro, login, BCrypt |
| 5 | Perfil de jugador: CRUD con atributos 1-10 y promedio |
| 6 | Partidos: CRUD, solo Admin puede crear |
| 7 | Inscripción: unirse a partidos, estado Open → Full |
| 8 | Balanceo: algoritmo greedy por promedio de habilidades |
| 9 | Costos: división del precio de la cancha, confirmación de pagos |

---

## Levantar la API

```bash
# Desde la raíz del proyecto
dotnet run --project src/FulboUY.API
```

Swagger disponible en: **http://localhost:5089/swagger**

---

## Flujo completo de prueba

### Paso 1 — Registrar un Admin

```http
POST http://localhost:5089/api/auth/register
Content-Type: application/json

{
  "email": "admin@fulbouy.com",
  "password": "Admin1234"
}
```

**Respuesta (201):**
```json
{
  "token": "eyJhbGci...",
  "email": "admin@fulbouy.com",
  "role": "Player",
  "expiresAt": "2026-03-23T..."
}
```

> **Nota:** Por ahora todos se registran como `Player`. Para hacer admin, hay que cambiar el rol directamente en la base de datos:
> ```sql
> UPDATE Users SET Role = 1 WHERE Email = 'admin@fulbouy.com';
> -- Role: 0 = Player, 1 = Admin
> ```

Guarda el `token` — lo necesitás en todos los siguientes pasos.

---

### Paso 2 — Registrar jugadores

Registrá al menos 10 jugadores para poder llenar un partido:

```http
POST http://localhost:5089/api/auth/register
Content-Type: application/json

{
  "email": "jugador1@test.com",
  "password": "Password1"
}
```

Repetí para `jugador2@test.com` ... `jugador10@test.com`.

---

### Paso 3 — Crear perfiles de jugador

Para cada jugador, hacé login y creá su perfil.

**Login:**
```http
POST http://localhost:5089/api/auth/login
Content-Type: application/json

{
  "email": "jugador1@test.com",
  "password": "Password1"
}
```

**Crear perfil** (con el token del jugador):
```http
POST http://localhost:5089/api/players
Authorization: Bearer {token_del_jugador}
Content-Type: application/json

{
  "name": "Juan Pérez",
  "speed": 8,
  "defense": 5,
  "passing": 7,
  "shooting": 9
}
```

**Respuesta (201):**
```json
{
  "id": "uuid...",
  "name": "Juan Pérez",
  "speed": 8,
  "defense": 5,
  "passing": 7,
  "shooting": 9,
  "averageSkill": 7.25
}
```

---

### Paso 4 — Crear un partido (requiere token de Admin)

```http
POST http://localhost:5089/api/matches
Authorization: Bearer {token_del_admin}
Content-Type: application/json

{
  "location": "Complejo Urbano - Cancha 3",
  "date": "2026-04-01T20:00:00",
  "fieldCost": 2000,
  "maxPlayers": 10
}
```

**Respuesta (201):**
```json
{
  "id": "match-uuid...",
  "location": "Complejo Urbano - Cancha 3",
  "date": "2026-04-01T20:00:00",
  "fieldCost": 2000.00,
  "maxPlayers": 10,
  "status": "Open",
  "participantCount": 0
}
```

Guardá el `id` del partido.

---

### Paso 5 — Inscribir jugadores al partido

Con el token de cada jugador:

```http
POST http://localhost:5089/api/matches/{match-id}/join
Authorization: Bearer {token_del_jugador}
```

Repetí esto con los 10 jugadores. Cuando se inscriba el décimo, el partido cambia a `status: "Full"`.

---

### Paso 6 — Ver participantes

```http
GET http://localhost:5089/api/matches/{match-id}/participants
Authorization: Bearer {token_cualquiera}
```

**Respuesta:**
```json
[
  {
    "id": "...",
    "playerProfileId": "...",
    "playerName": "Juan Pérez",
    "teamNumber": 0,
    "hasAid": false,
    "joinedAt": "..."
  },
  ...
]
```

---

### Paso 7 — Balancear equipos (solo Admin)

```http
POST http://localhost:5089/api/matches/{match-id}/balance-teams
Authorization: Bearer {token_del_admin}
```

**Respuesta:**
```json
{
  "team1": {
    "teamNumber": 1,
    "players": [...],
    "averageSkill": 7.15
  },
  "team2": {
    "teamNumber": 2,
    "players": [...],
    "averageSkill": 6.90
  },
  "skillDifference": 0.25
}
```

El algoritmo ordena por promedio de habilidades (desc) y asigna alternando: el mejor va al equipo 1, el segundo al equipo 2, el tercero al equipo 1, etc.

---

### Paso 8 — Ver división de costos

```http
GET http://localhost:5089/api/matches/{match-id}/cost-split
Authorization: Bearer {token_cualquiera}
```

**Respuesta:**
```json
{
  "matchId": "...",
  "totalCost": 2000.00,
  "playerCount": 10,
  "costPerPlayer": 200.00,
  "paidCount": 0,
  "pendingCount": 10
}
```

---

### Paso 9 — Confirmar pago de un jugador

Necesitás el `id` del participante (lo obtenés de `/participants`):

```http
POST http://localhost:5089/api/matches/{match-id}/participants/{participant-id}/pay
Authorization: Bearer {token_cualquiera}
```

---

### Paso 10 — Ver estado de pagos

```http
GET http://localhost:5089/api/matches/{match-id}/payment-status
Authorization: Bearer {token_cualquiera}
```

**Respuesta:**
```json
[
  { "participantId": "...", "playerName": "Juan Pérez", "hasAid": true },
  { "participantId": "...", "playerName": "Carlos García", "hasAid": false },
  ...
]
```

---

## Endpoints completos

### Auth
| Método | Endpoint | Auth | Descripción |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | No | Registrar usuario |
| POST | `/api/auth/login` | No | Login, devuelve JWT |

### Perfil de jugador
| Método | Endpoint | Auth | Descripción |
|--------|----------|------|-------------|
| POST | `/api/players` | JWT | Crear perfil |
| GET | `/api/players/me` | JWT | Mi perfil |
| GET | `/api/players/{id}` | JWT | Perfil por ID |
| PUT | `/api/players/{id}` | JWT (dueño) | Actualizar perfil |

### Partidos
| Método | Endpoint | Auth | Descripción |
|--------|----------|------|-------------|
| POST | `/api/matches` | Admin | Crear partido |
| GET | `/api/matches` | JWT | Listar partidos |
| GET | `/api/matches/{id}` | JWT | Detalle del partido |
| POST | `/api/matches/{id}/join` | JWT | Inscribirse |
| GET | `/api/matches/{id}/participants` | JWT | Listar inscriptos |
| POST | `/api/matches/{id}/balance-teams` | Admin | Balancear equipos |
| GET | `/api/matches/{id}/teams` | JWT | Ver equipos |
| GET | `/api/matches/{id}/cost-split` | JWT | Ver costo por jugador |
| POST | `/api/matches/{id}/participants/{pid}/pay` | JWT | Confirmar pago |
| GET | `/api/matches/{id}/payment-status` | JWT | Estado de pagos |

---

## Usar Swagger con JWT

1. Abrí **http://localhost:5089/swagger**
2. Hacé `POST /api/auth/register` o `POST /api/auth/login`
3. Copiá el `token` de la respuesta
4. Hacé click en el botón **Authorize** (candado arriba a la derecha)
5. Ingresá: `Bearer {tu_token}`
6. Confirmá — ahora todos los requests incluyen el token

---

## Errores comunes

| Error | Causa | Solución |
|-------|-------|----------|
| 401 Unauthorized | Token inválido o no enviado | Agregar `Bearer {token}` en el header |
| 403 Forbidden | Endpoint requiere rol Admin | Cambiar rol en BD o usar token de Admin |
| 409 Conflict | Ya inscripto / email duplicado / ya pagó | El recurso ya existe |
| 400 Bad Request | Partido no abierto, sin perfil de jugador, fecha pasada | Ver mensaje de error en la respuesta |
| 404 Not Found | ID no existe | Verificar el UUID del partido/jugador |

---

## Estructura del proyecto

```
FulboUY/
├── src/
│   ├── FulboUY.API/           # Controllers, DTOs, Program.cs
│   ├── FulboUY.Application/   # Services, Interfaces, DTOs internos
│   ├── FulboUY.Domain/        # Entidades, Enums
│   └── FulboUY.Infrastructure/ # Repositorios, AppDbContext, Migraciones
├── CLAUDE.md
└── FulboUY.sln
```
