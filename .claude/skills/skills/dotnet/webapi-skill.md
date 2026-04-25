# ASP.NET Core Web API Skill

Goal:
Implement REST controllers following clean architecture principles.

Architecture:
WebApi → BusinessLogic → DataAccess → Domain

Rules:

- Controllers must be thin
- Controllers must not contain business logic
- Controllers only orchestrate HTTP request and response
- Business rules belong in Logic layer
- Data access belongs in DataAccess layer
- Use Dependency Injection for services

Controller responsibilities:

- Receive HTTP request
- Validate input DTO
- Call service in BusinessLogic
- Return appropriate HTTP response

HTTP status codes:
200 OK → successful GET
201 Created → successful POST
204 NoContent → successful DELETE or PUT with no body
400 BadRequest → invalid input
401 Unauthorized → missing or invalid token
403 Forbidden → wrong role
404 NotFound → resource does not exist
409 Conflict → duplicated resource
500 InternalServerError → unexpected error

Example controller:

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
private readonly IUserLogic \_userLogic;

    public UsersController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        var createdUser = _userLogic.RegisterCustomer(user);
        return Created("", createdUser);
    }

}

Validation rules:

- Validate null or empty fields
- Validate formats (email, password)
- Use DTOs for request and response
- Do not expose domain entities directly if sensitive data exists

Security:

- Use JWT authentication
- Protect endpoints with [Authorize]
- Allow anonymous access only when required

Best practices:

- One controller per entity
- One responsibility per endpoint
- Use meaningful route names
- Avoid logic duplication
