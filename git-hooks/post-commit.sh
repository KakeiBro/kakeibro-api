#!/bin/bash

cd ./Kakeibro.API || exit

echo "Running appsettings backup restoration..."

jsonFilePath="./src/KakeiBro.API/appsettings.json"
jsonBakFilePath="./src/KakeiBro.API/appsettings.bak.json"

# Replace the file with the backup
if [ -f "$jsonBakFilePath" ]; then
    cp -f "$jsonBakFilePath" "$jsonFilePath"
    rm -f "$jsonBakFilePath"
else
    echo "Backup file not found: $jsonBakFilePath"
fi

echo "Post-commit operations done!"
