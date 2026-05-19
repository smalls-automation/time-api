var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/time", () =>
{
    return Results.Json(new { utc_time = DateTime.UtcNow.ToString("o").Replace("+00:00", "Z") });
});

app.Run();
