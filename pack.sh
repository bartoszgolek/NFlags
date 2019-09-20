#!/usr/bin/env bash

function join_by { local IFS="$1"; shift; echo "$*"; }

TAG=$(git describe --tags)
IFS='-'
read -ra VERSION_A <<< "$TAG"
IFS=' '
if [[ ${#VERSION_A[@]} -eq 1 ]]; then
    dotnet pack -c Release NFlags/NFlags.csproj /p:Version=${TAG} -o $(pwd)
else
    SUFFIX_A="${VERSION_A[@]:1}"
    VERSION_SUFFIX=$(join_by - ${SUFFIX_A})
    dotnet pack -c Release NFlags/NFlags.csproj /p:Version=${TAG} --version-suffix ${VERSION_SUFFIX} -o $(pwd)
fi
