#!/bin/bash

# Change to the Kakeibro.API directory
cd ./Kakeibro.API || { echo "Failed to change directory"; exit 1; }

echo "Running .NET format check..."
dotnet format --verify-no-changes
if [ $? -ne 0 ]; then
    exit 1
fi

echo "Running .NET build check..."
dotnet build
if [ $? -ne 0 ]; then
    exit 1
fi

echo "Running .NET test check..."
dotnet test
if [ $? -ne 0 ]; then
    exit 1
fi

echo "Running .NET outdated package check... (warning)"
dotnet list package --outdated
if [ $? -ne 0 ]; then
    exit 1
fi

echo "Running .NET migrations..."
# Uncomment the following line to enable migrations check
# dotnet ef migrations list
if [ $? -ne 0 ]; then
    exit 1
fi

echo "Running .NET vulnerability check..."
dotnet list package --vulnerable
if [ $? -ne 0 ]; then
    exit 1
fi

echo "Pre-push checks passed!"
