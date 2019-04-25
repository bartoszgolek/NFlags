#!/usr/bin/env bash

#exit if any command fails
set -e

if [ -z "$1" ]; then
    echo dotnet test ./NFlags.Tests/NFlags.Tests.csproj
    dotnet test ./NFlags.Tests/NFlags.Tests.csproj
else
    echo dotnet test -f $1 ./NFlags.Tests/NFlags.Tests.csproj
    dotnet test -f $1 ./NFlags.Tests/NFlags.Tests.csproj
fi