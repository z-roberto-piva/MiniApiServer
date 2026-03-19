param(
    [string]$AppConnectionString
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$infrastructureProject = Join-Path $repoRoot "src\MiniApiServer.Infrastructure\MiniApiServer.Infrastructure.csproj"
$apiProject = Join-Path $repoRoot "src\MiniApiServer.Api\MiniApiServer.Api.csproj"
$apiAppSettingsPath = Join-Path $repoRoot "src\MiniApiServer.Api\appsettings.json"
$dbContext = "MiniApiServer.Infrastructure.Persistence.MiniApiServerDbContext"

if (-not $AppConnectionString) {
    if (-not (Test-Path $apiAppSettingsPath)) {
        throw "Configuration file not found: $apiAppSettingsPath"
    }

    $appSettings = Get-Content -Path $apiAppSettingsPath -Raw | ConvertFrom-Json
    $AppConnectionString = $appSettings.ConnectionStrings.AppPostgres

    if ([string]::IsNullOrWhiteSpace($AppConnectionString)) {
        throw "ConnectionStrings.AppPostgres was not found in $apiAppSettingsPath"
    }
}

$env:MINI_API_SERVER_APP_CONNECTION_STRING = $AppConnectionString

dotnet ef database update `
    --project $infrastructureProject `
    --startup-project $apiProject `
    --context $dbContext
