var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/time", () =>
{
    return Results.Json(new { utc_time = DateTime.UtcNow.ToString("o") + "Z" });
});

app.Run();
