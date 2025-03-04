Set-Location .\Kakeibro.API

Write-Host "Running appsettings backup restoration..."

$jsonFilePath = ".\src\KakeiBro.API\appsettings.json"
$jsonBakFilePath = ".\src\KakeiBro.API\appsettings.bak.json"

# Replace the file with the backup
if (Test-Path $jsonBakFilePath) {
    Copy-Item -Path $jsonBakFilePath -Destination $jsonFilePath -Force
    Remove-Item -Path $jsonBakFilePath -Force
} else {
    Write-Host "Backup file not found: $jsonBakFilePath"
}

Write-Host "Post-commit operations done!"