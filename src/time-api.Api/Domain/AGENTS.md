# Domain/AGENTS.md

Domain entities are plain C# classes. No business logic, no service dependencies.
EF Core maps them to the database via `AppDbContext`.

## Naming
- File: `{Entity}.cs`
- Class: `{Entity}` (no suffix)
- Location: `src/time-api.Api/Domain/`

## Structure
```csharp
public class {Entity}
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK relationship:
    public Guid OwnerId { get; set; }
    public {Owner} Owner { get; set; } = null!;
}
```

## Rules
- Always include `Id` (Guid) and `CreatedAt` (DateTime UTC)
- Use `required` for non-nullable string properties
- Navigation properties use `null!` initialiser — never mark them nullable
- No methods on entities — behaviour belongs in services
- No data annotations for validation — use FluentValidation in the service layer

## Adding a new entity
1. Create `src/time-api.Api/Domain/{Entity}.cs`
2. Add `DbSet<{Entity}> {Entity}s {{ "{" }} get; set; {{ "}" }}` to `AppDbContext`
3. Create a migration: `dotnet ef migrations add Add{Entity}` (run from repo root)
4. Do NOT run `dotnet ef database update` — migrations run on startup
