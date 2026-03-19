# Mini Api Server

Per applicare le migrazion usare lo script apply-migrations.ps come segue:

```ps
.\scripts\apply-migrations.ps1 -AppConnectionString "Host=localhost;Port=5432;Database=mini_api_server;Username=...;Password=..."
```

Per verificare lo stato delle migrations

```ps
dotnet ef migrations list `
∙   --project src\MiniApiServer.Infrastructure\MiniApiServer.Infrastructure.csproj `
∙   --startup-project src\MiniApiServer.Api\MiniApiServer.Api.csproj `
∙   --context MiniApiServer.Infrastructure.Persistence.MiniApiServerDbContext
```
