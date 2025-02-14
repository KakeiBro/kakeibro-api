#!/bin/bash

set -e  # Exit immediately if a command exits with a non-zero status

# Change to the specified directory
cd "$(dirname "$0")/Kakeibro.API"

# Set environment variables
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# Restore dotnet tools
dotnet tool restore
if [ $? -ne 0 ]; then exit $?
fi

# Run Cake script
dotnet cake "$@"
if [ $? -ne 0 ]; then exit $?
fi

cd "$(dirname "$0")"