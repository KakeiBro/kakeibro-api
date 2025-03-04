Set-Location .\Kakeibro.API

Write-Host "Running workstation appsettings restore..."

$jsonFilePath = ".\src\KakeiBro.API\appsettings.json"
$jsonBakFilePath = ".\src\KakeiBro.API\appsettings.bak.json"

# Replace the file with the backup
if (Test-Path $jsonBakFilePath) {
    Copy-Item -Path $jsonBakFilePath -Destination $jsonFilePath -Force
} else {
    Write-Host "Backup file not found: $jsonBakFilePath"
}

Remove-Item -Path $jsonBakFilePath -Force

Write-Host "Post-commit operations done!"