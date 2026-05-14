# Services/AGENTS.md

Services own all business logic. Every service has an interface and one
implementation. Controllers and other services depend on the interface only.

## Naming
- Interface: `I{Resource}Service.cs`
- Implementation: `{Resource}Service.cs`
- Both in `src/time-api.Api/Services/`

## Structure
```csharp
public interface I{Resource}Service
{
    Task<Result<{Resource}>> GetAsync(Guid id, CancellationToken ct);
    Task<Result<{Resource}>> CreateAsync(Create{Resource}Request request, CancellationToken ct);
}

public class {Resource}Service(I{Resource}Repository repository) : I{Resource}Service
{
    public async Task<Result<{Resource}>> GetAsync(Guid id, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        return entity is null
            ? Result<{Resource}>.Fail($"{id} not found")
            : Result<{Resource}>.Ok(entity);
    }
}
```

## Rules
- Return `Result<T>` for any operation that has a meaningful failure mode
- Inject repository interfaces only — never `AppDbContext` directly
- Keep methods under 15 lines; extract named private helpers for longer logic
- No HTTP types (IActionResult, HttpContext, IFormFile) inside services
- No static methods — everything goes through the injected interface

## Adding a new service
1. Create `I{Resource}Service.cs` and `{Resource}Service.cs` in `Services/`
2. Register in `Program.cs`: `builder.Services.AddScoped<I{Resource}Service, {Resource}Service>();`
