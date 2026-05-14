---
name: add-endpoint
description: Add a new REST endpoint to an existing controller and wire it through the service layer
---

# Skill: Add Endpoint

## Steps

1. **Define the request/response models** in `src/time-api.Api/Models/`
   - Request: `Create{Resource}Request.cs` or `Update{Resource}Request.cs`
   - Response: `{Resource}Response.cs`
   - Use `record` types with `required` properties

2. **Add the service method** to `I{Resource}Service` and `{Resource}Service`
   - Return `Result<{Resource}Response>`
   - Keep under 15 lines; extract helpers if longer

3. **Add the controller action** to `{Resource}Controller`
   - Correct HTTP verb attribute (`[HttpPost]`, `[HttpPut("{id:guid}")]`, etc.)
   - Call the service, map `Result<T>` to `ActionResult<T>`
   - Pass `CancellationToken ct` through

4. **Verify**: `dotnet build` exits 0

## Do not
- Add a new controller for a single endpoint on an existing resource
- Put any logic in the controller beyond service delegation
- Modify files outside Controllers/, Services/, and Models/
