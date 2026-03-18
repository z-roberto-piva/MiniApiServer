param(
    [string]$AppConnectionString
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$infrastructureProject = Join-Path $repoRoot "src\MiniApiServer.Infrastructure\MiniApiServer.Infrastructure.csproj"
$apiProject = Join-Path $repoRoot "src\MiniApiServer.Api\MiniApiServer.Api.csproj"
$dbContext = "MiniApiServer.Infrastructure.Persistence.MiniApiServerDbContext"

if ($AppConnectionString) {
    $env:MINI_API_SERVER_APP_CONNECTION_STRING = $AppConnectionString
}

dotnet ef database update `
    --project $infrastructureProject `
    --startup-project $apiProject `
    --context $dbContext
