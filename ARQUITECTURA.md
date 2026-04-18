# FulboUY — Guía completa del proyecto

Este documento explica **qué es cada carpeta, cada archivo, y cada concepto** del proyecto.
Es tu mapa para entender todo lo que hay acá.

---

## La gran imagen — ¿Qué construimos?

FulboUY tiene **dos partes**:

```
FulboUY/
│
├── src/          ← BACKEND  (.NET 8, C#)  → corre en http://localhost:5089
│
└── client/       ← FRONTEND (React)       → corre en http://localhost:5173
```

El **backend** es el cerebro: maneja los datos, la base de datos, la lógica del negocio.
El **frontend** es la cara: lo que ves en el navegador.

Se hablan así:
```
Navegador (React) ──── HTTP requests ────► API (.NET) ──── SQL ────► Base de datos
```

---

## Carpetas de la raíz del proyecto

```
fulbouy/
├── src/                 ← Todo el código del backend (.NET)
├── client/              ← Todo el código del frontend (React)
├── FulboUY.sln          ← Archivo de solución de Visual Studio / Rider
├── CLAUDE.md            ← Instrucciones para Claude Code (el asistente de IA)
├── GUIA_PRUEBAS.md      ← Cómo probar los endpoints
├── README.md            ← Este archivo
├── README.txt           ← Descripción general del proyecto
└── .idea/               ← Configuración del IDE Rider (no tocar a mano)
```

### ¿Qué es el `.sln`?
Es el archivo que le dice a Rider/Visual Studio "este proyecto tiene estas 4 partes".
Es como el índice de un libro. Lo abrís en Rider y él carga todo.

---

## El Backend — carpeta `src/`

El backend está dividido en **4 proyectos**. Cada uno tiene una responsabilidad única.
Esto se llama **arquitectura en capas**.

```
src/
├── FulboUY.API/           ← Capa 1: La puerta de entrada (endpoints HTTP)
├── FulboUY.Application/   ← Capa 2: Las reglas del negocio
├── FulboUY.Domain/        ← Capa 3: Las entidades (los datos del mundo real)
└── FulboUY.Infrastructure/← Capa 4: La base de datos
```

### Analogía con un restaurante

| Capa | Equivalente en un restaurante |
|------|-------------------------------|
| API | El mozo — recibe el pedido del cliente |
| Application | El chef — decide cómo preparar el plato |
| Domain | Los ingredientes — lo que existe en el mundo real |
| Infrastructure | La heladera/despensa — donde se guarda todo |

---

## Capa 1: `FulboUY.API/` — La puerta de entrada

```
FulboUY.API/
├── Controllers/
│   ├── AuthController.cs           ← Endpoints de login y registro
│   ├── MatchController.cs          ← Endpoints de partidos
│   └── PlayerProfileController.cs  ← Endpoints de perfiles de jugador
├── DTOs/
│   ├── Auth/                       ← Qué datos se envían/reciben en auth
│   ├── Match/                      ← Qué datos se envían/reciben en partidos
│   └── PlayerProfile/              ← Qué datos se envían/reciben en perfiles
├── Program.cs                      ← Punto de entrada de la app (el "main")
└── appsettings.json                ← Configuración (base de datos, JWT, etc.)
```

### ¿Qué es un Controller?

Un Controller es una clase C# que **escucha requests HTTP** y devuelve respuestas.

Ejemplo real de `AuthController.cs`:
```
Alguien hace POST http://localhost:5089/api/auth/login
         ↓
AuthController recibe el request
         ↓
Llama al AuthService para verificar el password
         ↓
Devuelve el token JWT (o un error 401)
```

Cada método del controller corresponde a un endpoint:
- `[HttpPost("register")]` → `POST /api/auth/register`
- `[HttpGet("{id}")]` → `GET /api/matches/123`

### ¿Qué es un DTO?

DTO significa **Data Transfer Object**. Es un objeto simple que define exactamente
qué datos viajan entre el cliente y el servidor.

Ejemplo:
```csharp
// Lo que el cliente MANDA para hacer login:
public class LoginRequest {
    public string Email { get; set; }
    public string Password { get; set; }
}

// Lo que el servidor DEVUELVE después del login:
public class AuthResponse {
    public string Token { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }  // "Admin" o "Player"
}
```

Los DTOs son diferentes a las entidades de la base de datos — **nunca exponemos
directamente los datos internos**.

### ¿Qué es `Program.cs`?

Es el arranque de toda la aplicación. Ahí se configura:
- La conexión a la base de datos
- La autenticación JWT
- Los servicios disponibles (inyección de dependencias)
- Swagger (la interfaz de documentación)

---

## Capa 2: `FulboUY.Application/` — Las reglas del negocio

```
FulboUY.Application/
├── Services/
│   ├── AuthService.cs              ← Lógica de registro, login, JWT
│   ├── MatchService.cs             ← Lógica de partidos e inscripción
│   ├── PlayerProfileService.cs     ← Lógica de perfiles de jugador
│   ├── TeamBalancingService.cs     ← Algoritmo de balanceo de equipos
│   └── CostSplitService.cs         ← Lógica de división de costos
├── Interfaces/
│   ├── IAuthService.cs             ← Contrato del servicio de auth
│   ├── IMatchService.cs            ← Contrato del servicio de partidos
│   ├── IUserRepository.cs          ← Contrato del repositorio de usuarios
│   ├── IMatchRepository.cs         ← Contrato del repositorio de partidos
│   └── ...                         ← Un contrato por cada servicio/repositorio
├── DTOs/                           ← DTOs internos (entre capas)
└── Settings/
    └── JwtSettings.cs              ← Configuración del JWT (secret, issuer, etc.)
```

### ¿Qué es un Service?

Un Service contiene la **lógica del negocio**. Es donde viven las reglas:

Ejemplo de `MatchService.cs`:
```
¿Puedo unirme a este partido?
  → ¿El partido está "Open"? Si no → error
  → ¿Tengo un perfil de jugador? Si no → error
  → ¿Ya estoy inscripto? Si sí → error
  → Todo ok → crear la inscripción en la BD
  → ¿El partido se llenó? → cambiar estado a "Full"
```

### ¿Qué es una Interface?

Una interface es un **contrato** que dice "esta clase debe tener estos métodos".

```csharp
// El contrato:
public interface IMatchService {
    Task<MatchDto> CreateAsync(...);
    Task<MatchDto?> GetByIdAsync(Guid id);
    Task JoinMatchAsync(Guid matchId, Guid userId);
}

// La implementación concreta:
public class MatchService : IMatchService {
    // Acá se implementan esos métodos con la lógica real
}
```

¿Para qué sirve esto? Para que el Controller solo conozca el contrato, no la implementación.
Así podés cambiar la implementación sin tocar el Controller.

### ¿Cómo funciona el balanceador de equipos?

**Algoritmo greedy:**

1. Toma todos los jugadores inscriptos al partido
2. Calcula el promedio de habilidades de cada uno: `(Speed + Defense + Passing + Shooting) / 4`
3. Los ordena de mayor a menor promedio
4. Los asigna alternando: el mejor va al Equipo 1, el segundo al Equipo 2, el tercero al Equipo 1, etc.

```
Jugadores ordenados por habilidad:
  Juan   → 9.0  → Equipo 1
  Pedro  → 8.5  → Equipo 2
  María  → 8.0  → Equipo 1
  Carlos → 7.5  → Equipo 2
  Ana    → 7.0  → Equipo 1
  ...
```

Resultado: los dos equipos tienen promedios muy similares.

---

## Capa 3: `FulboUY.Domain/` — Las entidades

```
FulboUY.Domain/
├── Entities/
│   ├── User.cs             ← Un usuario del sistema
│   ├── PlayerProfile.cs    ← El perfil futbolístico del jugador
│   ├── Match.cs            ← Un partido
│   ├── MatchParticipant.cs ← La relación "jugador inscripto en partido"
│   └── InviteLink.cs       ← Un link de invitación a un partido
└── Enums/
    ├── UserRole.cs         ← Admin = 1, Player = 0
    └── MatchStatus.cs      ← Open, Full, Played
```

### ¿Qué es una Entidad?

Una entidad es una clase C# que representa **algo del mundo real** y se guarda en la BD.

```csharp
public class Match {
    public Guid Id { get; set; }          // Identificador único
    public string Location { get; set; }  // "Complejo Urbano - Cancha 3"
    public DateTime Date { get; set; }    // Cuándo se juega
    public decimal FieldCost { get; set; }// Precio de la cancha
    public int MaxPlayers { get; set; }   // 10, 14, etc.
    public MatchStatus Status { get; set; }// Open → Full → Played
}
```

Cada propiedad = una columna en la tabla de la base de datos.

### Las relaciones entre entidades

```
User ──────────── tiene ──────────── PlayerProfile
                                           │
                                           │ participa en
                                           ▼
Match ──────────── tiene ──────────── MatchParticipant
  │                                        │
  └──── tiene ──── InviteLink              └── TeamNumber (1 o 2)
                                               HasPaid (true/false)
```

---

## Capa 4: `FulboUY.Infrastructure/` — La base de datos

```
FulboUY.Infrastructure/
├── Data/
│   └── AppDbContext.cs          ← La conexión a SQL Server (EF Core)
├── Repositories/
│   ├── UserRepository.cs        ← CRUD de usuarios en la BD
│   ├── MatchRepository.cs       ← CRUD de partidos en la BD
│   ├── PlayerProfileRepository.cs
│   ├── MatchParticipantRepository.cs
│   └── InviteLinkRepository.cs
└── Migrations/
    └── 20260322_InitialCreate/  ← El script que creó las tablas en SQL Server
```

### ¿Qué es un Repository?

Un Repository es quien **habla con la base de datos**. Solo sabe leer y escribir datos,
sin lógica de negocio.

```csharp
// MatchRepository solo hace consultas SQL (via EF Core):
public async Task<Match?> GetByIdAsync(Guid id) =>
    await _context.Matches.FindAsync(id);

public async Task<Match> CreateAsync(Match match) {
    _context.Matches.Add(match);
    await _context.SaveChangesAsync();
    return match;
}
```

### ¿Qué es EF Core?

Entity Framework Core es un ORM (Object-Relational Mapper). Convierte las clases C#
en tablas SQL automáticamente. Vos escribís C#, él genera el SQL.

```csharp
// Vos escribís esto:
var matches = await _context.Matches.Where(m => m.Status == MatchStatus.Open).ToListAsync();

// EF Core genera esto (SQL):
SELECT * FROM Matches WHERE Status = 0
```

### ¿Qué son las Migrations?

Las migraciones son el historial de cambios en la base de datos.
Cuando ejecutaste `dotnet ef migrations add InitialCreate`, EF Core generó
el script SQL que crea todas las tablas. `dotnet ef database update` lo ejecutó.

---

## El Frontend — carpeta `client/`

```
client/
├── src/
│   ├── api/
│   │   ├── client.js       ← Instancia de axios con el token JWT automático
│   │   ├── auth.js         ← login() y register()
│   │   ├── matches.js      ← Todo lo de partidos
│   │   └── players.js      ← Todo lo de perfiles
│   ├── context/
│   │   └── AuthContext.jsx ← Estado global: usuario logueado, token, rol
│   ├── components/
│   │   ├── Navbar.jsx      ← Barra de navegación superior
│   │   └── PrivateRoute.jsx← Redirige a /login si no hay sesión
│   └── pages/
│       ├── LoginPage.jsx       ← Pantalla de login
│       ├── RegisterPage.jsx    ← Pantalla de registro
│       ├── MatchesPage.jsx     ← Lista de partidos
│       ├── MatchDetailPage.jsx ← Detalle + unirse + equipos + pagos
│       ├── CreateMatchPage.jsx ← Formulario para crear partido (Admin)
│       └── ProfilePage.jsx     ← Perfil del jugador con sliders
├── package.json            ← Dependencias de Node.js
└── vite.config.js          ← Configuración de Vite (proxy al backend)
```

### ¿Qué es el proxy de Vite?

El frontend corre en `:5173` y el backend en `:5089`.
Sin el proxy, el navegador bloquearía los requests por CORS.

El proxy en `vite.config.js` dice: "cualquier request a `/api/...` redirigilos al puerto 5089".

```
React hace:  GET /api/matches
Vite proxy:  → GET http://localhost:5089/api/matches
```

### ¿Qué es el AuthContext?

Es el estado global del usuario. En vez de pasar el token por cada componente,
cualquier página puede preguntar "¿hay alguien logueado?" y "¿es Admin?".

```jsx
// Desde cualquier página:
const { user, isAdmin, login, logout } = useAuth()
```

---

## El flujo completo de una request

Ejemplo: **un jugador se une a un partido**

```
1. Usuario hace click en "Unirme al partido" (React)
         ↓
2. React llama a joinMatch(id) en src/api/matches.js
         ↓
3. axios hace POST /api/matches/{id}/join con el JWT en el header
         ↓
4. Vite proxy redirige a http://localhost:5089/api/matches/{id}/join
         ↓
5. MatchController.Join() recibe el request (.NET)
         ↓
6. Extrae el userId del token JWT
         ↓
7. Llama a matchService.JoinMatchAsync(matchId, userId)
         ↓
8. MatchService verifica:
   → ¿Partido existe? ¿Está Open? ¿Usuario tiene perfil? ¿Ya inscripto?
         ↓
9. MatchParticipantRepository.CreateAsync() guarda en SQL Server
         ↓
10. Si el partido se llenó → cambia Status a Full
         ↓
11. Devuelve 201 Created con los datos del participante
         ↓
12. React actualiza la pantalla
```

---

## ¿Cómo funciona la autenticación JWT?

JWT = JSON Web Token. Es una forma de identificar usuarios sin guardar sesiones.

```
1. Usuario hace login con email + password
         ↓
2. El servidor verifica el password (BCrypt)
         ↓
3. Si es correcto, genera un TOKEN firmado con una clave secreta:
   eyJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6InVzdWFyaW9AZW1haWwuY29tIiwicm9sZSI6IlBsYXllciJ9.xxx
         ↓
4. El token se guarda en localStorage del navegador
         ↓
5. En cada request posterior, el token se manda en el header:
   Authorization: Bearer eyJhbGci...
         ↓
6. El servidor verifica la firma y extrae el email y rol del usuario
```

El token contiene adentro: email, rol (Admin/Player), ID del usuario, fecha de expiración.

---

## ¿Cómo probar los endpoints sin el frontend?

Tenés dos opciones:

### Opción A — Swagger (recomendado)
Abrí `http://localhost:5089/swagger` con el backend corriendo.
Es una interfaz web que muestra todos los endpoints y te deja probarlos.

1. Registrate con `POST /api/auth/register`
2. Copiá el token de la respuesta
3. Hacé click en **Authorize** (arriba a la derecha)
4. Ingresá `Bearer {el-token}`
5. Ahora podés probar cualquier endpoint autenticado

### Opción B — El archivo `.http`
En `src/FulboUY.API/FulboUY.API.http` hay ejemplos de requests que podés
ejecutar directo desde Rider haciendo click en el triángulo verde.

---

## Resumen de roles

| Rol | Puede hacer |
|-----|-------------|
| **Player** (default) | Crear perfil, ver partidos, unirse a partidos, confirmar su pago |
| **Admin** | Todo lo anterior + crear partidos + balancear equipos |

Para hacerte Admin:
```sql
USE FulboUY;
UPDATE Users SET Role = 1 WHERE Email = 'tu@email.com';
```
Luego volvé a hacer login para actualizar el token.

---

## Glosario rápido

| Término | Qué significa |
|---------|---------------|
| **API** | Interfaz que expone funciones via HTTP (GET, POST, PUT, DELETE) |
| **REST** | Convención para diseñar APIs usando HTTP de forma estándar |
| **Controller** | Clase que recibe requests HTTP y devuelve respuestas |
| **Service** | Clase con la lógica del negocio |
| **Repository** | Clase que habla con la base de datos |
| **Entity** | Clase que representa una tabla de la BD |
| **DTO** | Objeto que define qué datos viajan entre cliente y servidor |
| **Interface** | Contrato que define qué métodos debe tener una clase |
| **JWT** | Token firmado que identifica al usuario en cada request |
| **EF Core** | Herramienta que convierte clases C# en tablas SQL |
| **Migration** | Script que crea/modifica tablas en la BD |
| **Swagger** | Interfaz web para documentar y probar los endpoints |
| **Proxy** | Redirige requests del frontend al backend (evita problemas de CORS) |
| **Context** | En React: estado global accesible desde cualquier componente |
