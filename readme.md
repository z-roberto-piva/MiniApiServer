# Mini Api Server

## Preparazione del database

Connessione al database con utente principale

```SQL
CREATE ROLE mini_api_server WITH LOGIN PASSWORD 'sviluppo';
CREATE ROLE mini_api_server_hangfire WITH LOGIN PASSWORD 'sviluppo';

CREATE DATABASE mini_api_server OWNER mini_api_server;
CREATE DATABASE mini_api_server_hangfire OWNER mini_api_server_hangfire;
```

Connessione al database con l'utente mini_api_server

```SQL
CREATE SCHEMA mini_api_server;

```

Connessione al database con l'utente mini_api_server_hangfire

```SQL
CREATE SCHEMA mini_api_server_hangfire;

```

Avviare lo script sotto dalla root del progetto per creare la struttura delle tabelle mediante l'applicazione delle migrazioni.

## Migrations

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
