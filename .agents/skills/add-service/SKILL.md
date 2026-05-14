---
name: add-service
description: Create a new service interface and implementation and register it for dependency injection
---

# Skill: Add Service

## Steps

1. **Create the interface** `src/time-api.Api/Services/I{Resource}Service.cs`
   - One method per operation (Get, Create, Update, Delete)
   - All methods return `Task<Result<T>>`
   - All methods take `CancellationToken ct` as last parameter

2. **Create the implementation** `src/time-api.Api/Services/{Resource}Service.cs`
   - Primary constructor injection — inject `I{Resource}Repository` only
   - Implement every interface method
   - Return `Result<T>.Ok(value)` or `Result<T>.Fail(reason)` — never throw for expected failures

3. **Register in `Program.cs`**
   ```csharp
   builder.Services.AddScoped<I{Resource}Service, {Resource}Service>();
   ```
   Add this line alongside the existing service registrations.

4. **Verify**: `dotnet build` exits 0

## Do not
- Inject `AppDbContext` directly — always go through a repository interface
- Add static methods
- Create the repository in the same task unless the task explicitly says to
