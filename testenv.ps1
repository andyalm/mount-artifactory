#!/usr/bin/env pwsh -NoExit -Interactive -NoLogo

param(
    [switch]
    $Debug
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
Import-Module $([IO.Path]::Combine($PWD,'src','bin','Debug','net6.0','Module','MountArtifactory.psd1'))
cd artifactory: