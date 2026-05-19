# time-api

Tiny .NET 9 minimal API returning current UTC time.

## Tech Stack
- .NET 9
- Minimal APIs (no controllers)
- No database, no auth

## File Layout
```
time-api/
├── time-api.sln
├── time-api/
│   ├── time-api.csproj
│   ├── Program.cs
│   └── Properties/
│       └── launchSettings.json
├── Dockerfile
├── docker-compose.yml
├── .gitignore
└── AGENTS.md
```

## Running
```bash
dotnet run --project time-api
```

Endpoint: `GET http://localhost:5000/api/time`
