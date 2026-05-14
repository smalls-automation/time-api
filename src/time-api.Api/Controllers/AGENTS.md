# Controllers/AGENTS.md

Controllers are thin HTTP entry points. They validate input, delegate to one
service, and return a typed result. No business logic lives here.

## Naming
- File: `{Resource}Controller.cs`
- Class: `{Resource}Controller : ControllerBase`
- Route: `[Route("api/[controller]")]`
- One controller per resource, not per operation

## Structure
```csharp
[ApiController]
[Route("api/[controller]")]
public class {Resource}Controller(I{Resource}Service service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<{Resource}Response>> GetAsync(Guid id, CancellationToken ct)
    {
        var result = await service.GetAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
```

## Rules
- Constructor-inject the service interface only — nothing else
- Return `ActionResult<T>` — never raw objects or strings
- Every async method takes `CancellationToken ct` as the last parameter
- Never call repositories directly from a controller
- Apply `[Authorize]` at class level; override with `[AllowAnonymous]` per endpoint
- Use `Result<T>` returns from the service to decide the HTTP status code

## Adding a new controller
1. Create `src/time-api.Api/Controllers/{Resource}Controller.cs`
2. No registration needed — ASP.NET discovers controllers automatically via `AddControllers()`
