#Write-Host "Running .NET format check..."
#dotnet format --verify-no-changes
#if ($LASTEXITCODE -ne 0) { exit 1 }
#
#Write-Host "Running .NET build check..."
#dotnet build /warnaserror
#if ($LASTEXITCODE -ne 0) { exit 1 }
#
#Write-Host "Pre-commit checks passed!"
Write-Host "WE ARE AT WINDOWS!"