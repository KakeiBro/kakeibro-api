Set-Location .\Kakeibro.API

Write-Host "Running .NET format check..."
dotnet format --verify-no-changes
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Running .NET build check..."
dotnet build
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Running .NET test check..."
dotnet test
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Running Google OAuth credential sanitization..."

# Ensure jq is installed
if (-not (Get-Command jq -ErrorAction SilentlyContinue)) {
    Write-Host "Error: jq is not installed. Install it using 'choco install jq' (Windows)."
    exit 1
}

$jsonFilePath = ".\src\KakeiBro.API\appsettings.json"
$jsonBakFilePath = ".\src\KakeiBro.API\appsettings.bak.json"

# Create a backup of the original JSON
Copy-Item -Path $jsonFilePath -Destination $jsonBakFilePath -Force

# Use jq to modify the JSON and format it
$jqFilter = '(.GoogleAuth.ClientId, .GoogleAuth.RedirectUri, .GoogleAuth.JavascriptOrigin) |= ""'
jq $jqFilter $jsonFilePath | Out-File -Encoding utf8 "$jsonFilePath"

# Stage the modified file in Git
git add $jsonFilePath

Write-Host "Pre-commit checks passed!"