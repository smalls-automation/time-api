---
name: add-migration
description: Add a new EF Core entity and create a database migration
---

# Skill: Add Migration

## Steps

1. **Create the entity** in `src/time-api.Api/Domain/{Entity}.cs`
   - `Id` (Guid), `CreatedAt` (DateTime UTC) on every entity
   - Use `required` for non-nullable strings
   - Navigation properties initialised with `null!`

2. **Register the DbSet** in `AppDbContext.cs`
   ```csharp
   public DbSet<{Entity}> {Entity}s { get; set; }
   ```

3. **Create the migration** (run from repo root)
   ```bash
   dotnet ef migrations add Add{Entity}
   ```
   Verify the generated migration file in `Migrations/` looks correct before finishing.

4. **Verify**: `dotnet build` exits 0

## Do not
- Run `dotnet ef database update` — migrations apply automatically on startup
- Put business logic or computed columns in the migration file
- Modify existing migration files
