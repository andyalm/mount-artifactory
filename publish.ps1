$ErrorActionPreference='Stop'

dotnet publish MountArtifactory
Get-ChildItem -Recurse ./bin/MountArtifactory | Select-Object Name
Publish-Module -Path ./bin/MountArtifactory -NuGetApiKey $env:NuGetApiKey