#!/usr/bin/env bash

dotnet pack -c Release NFlags/NFlags.csproj -o $(pwd)
