Param(
    [string] $Configuration = "Debug",
    [string] $CodeDirectory = "Source",
    [string] $TestsDirectory = "Source",
    [string] $PackageOutputDirectory = "Build",
    [string] $Version,
    [string[]] $Package = @("Machine.Fakes","Machine.Fakes.FakeItEasy","Machine.Fakes.Moq","Machine.Fakes.NSubstitute","Machine.Fakes.Rhinomocks")
)

$tests = Get-ChildItem $TestsDirectory -Recurse -File -Filter "*.Specs.csproj"
$projects = Get-ChildItem $CodeDirectory -Recurse -File -Filter "*.csproj"

function Invoke-ExpressionExitCodeCheck([string] $command)
{
    Invoke-Expression $command

    if ($LASTEXITCODE -and $LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

# Patch version
if ($Version) {
    Write-Host "Patching versions to ${Version}..."

    $projects | ForEach {
        Write-Host "Updating version: $($_.FullName)"

        $foundVersion = $false; # replace only the first occurance of version (assumes package version is on top)

        (Get-Content $_.FullName -ErrorAction Stop) |
            Foreach-Object {
                $versionLine = '"version":\s*?".*?"'
                if (($foundVersion -ne $true) -and ($_ -match $versionLine)) {
                    $foundVersion = $true;
                    return $_ -replace '"version":\s*?".*?"',"""version"": ""$Version"""
                } else {
                    return $_
                }
            } |
            Set-Content $_.FullName -ErrorAction Stop
    }
}

# Build

Write-Host "Restoring packages..."
Invoke-ExpressionExitCodeCheck "dotnet restore Source\Machine.Fakes.sln" -ErrorAction Stop

Write-Host "Building in ${Configuration}..."
Invoke-ExpressionExitCodeCheck "dotnet build Source\Machine.Fakes.sln -c ${Configuration}" -ErrorAction Stop


# Test

Write-Host "Running tests..."

[bool] $testsFailed = $false
$tests | ForEach {
    Invoke-Expression "dotnet test $($_.FullName) -c ${Configuration}"

    if (!$testsFailed) {
        $testsFailed = $LASTEXITCODE -and $LASTEXITCODE -ne 0
    }
}

if ($testsFailed) {
    Write-Host -BackgroundColor Red -ForegroundColor Yellow "Tests failed!"
    exit -1
} else {
    Write-Host -BackgroundColor Green -ForegroundColor White "All good!"
}


# NuGet packaging

if ($Package) {
    $Package | ForEach {
        Write-Host "Creating nuget package in $CodeDirectory\$_\$PackageOutputDirectory"
        Invoke-ExpressionExitCodeCheck "dotnet pack ${CodeDirectory}\$($_) -c ${Configuration} -o ${PackageOutputDirectory} /p:PackageVersion=${Version}"
    }
}
