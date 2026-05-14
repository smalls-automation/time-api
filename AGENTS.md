# AGENTS.md — smalls-automation/time-api

## Stack
- Language: C# (.NET 9)
- Solution: `time-api.sln` at the repo root
- Main project: `src/time-api.Api/`
- Tests: `tests/time-api.Tests/` — xUnit + Moq (pre-created, already references src project)

## Commands
- Build: `dotnet build time-api.sln` (run from repo root — always name the .sln)
- Test: `dotnet test time-api.sln`
- Format: `dotnet format`
- New migration: `dotnet ef migrations add {Name}` (run from repo root)

## Project structure
```
time-api.sln          ← solution file at repo root — do NOT recreate
src/time-api.Api/     ← the only project directory — all source goes here
  time-api.Api.csproj ← project file — do NOT recreate
  Program.cs                    ← DI registration and middleware pipeline
  Controllers/                  ← HTTP entry points (see Controllers/AGENTS.md)
  Services/                     ← business logic + interfaces (see Services/AGENTS.md)
  Domain/                       ← EF Core entities (see Domain/AGENTS.md)
  Models/                       ← request/response DTOs
```

**CRITICAL:** All source files go inside `src/time-api.Api/` — never directly in `src/`.
Do not create files at `src/Program.cs`, `src/Controllers/…`, etc. — that creates a second
project and breaks the build with `MSB1011: more than one project or solution file`.

## Rules
1. Read the AGENTS.md in the directory you are working in before writing code
2. Follow existing patterns — find a similar file and match its structure exactly
3. Never modify `tests/` unless the task explicitly says to write tests
4. `dotnet build time-api.sln` must exit 0 before you finish
5. Register new services in `Program.cs` following the existing pattern
6. Never create a new .sln or .csproj — they were created by the scaffold

## Boundaries
- Do not modify `.github/`, `Dockerfile`, `docker-compose.yml`, or `.factory/`
- Do not add NuGet packages without explicit instruction
