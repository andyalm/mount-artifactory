#!/usr/bin/env pwsh -NoExit -Interactive -NoLogo

param(
    [Parameter(Mandatory=$true)]
    [string]
    $ArtifactoryEndpoint
)
$ErrorActionPreference='Stop'
$DebugPreference=$Debug ? 'Continue' : 'SilentlyContinue'
dotnet build
if(-not (Get-Alias ls -ErrorAction SilentlyContinue)) {
    New-Alias ls Get-ChildItem
}
if(-not (Get-Alias cat -ErrorAction SilentlyContinue)) {
    New-Alias cat Get-Content
}
$env:NO_MOUNT_ARTIFACTORY='1'
Import-Module $([IO.Path]::Combine($PWD,'src','bin','Debug','net6.0','Module','MountArtifactory.psd1'))
New-PSDrive -Name artifactory -PSProvider MountArtifactory -Root '' -ArtifactoryEndpoint $ArtifactoryEndpoint
cd artifactory:
