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
$jsonFilePath = ".\src\KakeiBro.API\appsettings.json"
$jsonBakFilePath = ".\src\KakeiBro.API\appsettings.bak.json"

# Read the JSON file
$jsonContent = Get-Content -Raw -Path $jsonFilePath | ConvertFrom-Json
$restoreContent = Get-Content -Raw -Path $jsonFilePath | ConvertFrom-Json

# Ensure sensitive credentials are set to an empty string
$jsonContent.GoogleAuth.ClientId = ""
$jsonContent.GoogleAuth.RedirectUri = ""
$jsonContent.GoogleAuth.JavascriptOrigin = ""

$jsonContent | ConvertTo-Json -Depth 10 | Set-Content -Path $jsonFilePath
$restoreContent | ConvertTo-Json -Depth 10 | Set-Content -Path $jsonBakFilePath

git add $jsonFilePath

Write-Host "Pre-commit checks passed!"