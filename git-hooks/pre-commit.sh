#!/bin/bash

cd ./Kakeibro.API || exit 1

echo "Running .NET format check..."
dotnet format --verify-no-changes
if [ $? -ne 0 ]; then exit 1; fi

echo "Running .NET build check..."
dotnet build
if [ $? -ne 0 ]; then exit 1; fi

echo "Running .NET test check..."
dotnet test
if [ $? -ne 0 ]; then exit 1; fi

echo "Running Google OAuth credential sanitization..."

# Ensure jq is installed
if ! command -v jq &> /dev/null; then
    echo "Error: jq is not installed. Install it using 'sudo apt install jq' (Linux) or 'brew install jq' (macOS)."
    exit 1
fi

jsonFilePath="./src/KakeiBro.API/appsettings.json"
jsonBakFilePath="./src/KakeiBro.API/appsettings.bak.json"

# Create a backup of the original JSON
cp -f "$jsonFilePath" "$jsonBakFilePath"

# Read entire file first to avoid truncation on the same pipeline command
jq '(.GoogleAuth.ClientId, .GoogleAuth.RedirectUri, .GoogleAuth.JavascriptOrigin, .GoogleAuth.ClientSecret) |= ""' "$jsonFilePath" > temp.json && mv temp.json "$jsonFilePath"

# Stage the modified file in Git
git add "$jsonFilePath"

echo "Pre-commit checks passed!"
