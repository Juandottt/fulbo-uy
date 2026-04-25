# JWT Authentication Skill

Goal:
Implement JWT-based authentication and authorization in ASP.NET Core 8.

Context:
The system requires login with email and password, and role-based access to endpoints.

Rules:

- Use JWT for authentication
- Generate token after successful login
- Include role claims in token
- Protect endpoints with [Authorize]
- Use role-based authorization where needed
- Keep authentication logic in BusinessLogic or dedicated auth service
- Controllers must only receive request and return response

Expected flow:

1. User sends email and password
2. Service validates credentials
3. If valid, generate JWT token
4. Token must include user id, email and role
5. Client uses Bearer token in Authorization header

Best practices:

- Store JWT settings in configuration
- Use strong secret key
- Set expiration time
- Return 401 for invalid credentials
- Return 403 for authenticated users without required role

Example claims:

- sub → user id
- email → user email
- role → Client / Dispatcher / Administrator

Security rules:

- Never expose password in responses
- Never store plain password if hashing is added later
- Anonymous access only where explicitly required
