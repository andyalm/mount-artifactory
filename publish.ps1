$ErrorActionPreference='Stop'

dotnet publish src
Get-ChildItem -Recurse ./bin/MountArtifactory | Select-Object Name
Publish-Module -Path ./bin/MountArtifactory -NuGetApiKey $env:NuGetApiKey