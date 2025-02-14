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

Write-Host "Running .NET outdated package check... (warning nature)"
dotnet list package --outdated
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Running .NET migrations..."
dotnet ef migrations list
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Running .NET vulnerability check..."
dotnet list package --vulnerable
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Pre-commit checks passed!"