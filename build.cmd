@echo off
powershell -NoProfile -ExecutionPolicy unrestricted -Command "& {Import-Module .\psake.psm1; Invoke-psake .\default.ps1 %*}"