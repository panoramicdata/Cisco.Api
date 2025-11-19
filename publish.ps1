#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Publishes the Cisco.Api NuGet package.

.DESCRIPTION
    This script performs the following steps:
    1. Checks that the git working directory is clean (porcelain)
    2. Runs unit tests (can be skipped with --skip-tests)
    3. Builds the project in Release mode
    4. Publishes the package to NuGet using the API key from nuget-key.txt
    5. Publishes symbols package (snupkg) for debugging support

.PARAMETER SkipTests
    Skip running unit tests before publishing

.EXAMPLE
    .\publish.ps1

.EXAMPLE
    .\publish.ps1 --skip-tests
#>

param(
    [switch]$SkipTests
)

$ErrorActionPreference = "Stop"

# Color output functions
function Write-Info {
    param([string]$Message)
    Write-Host "ℹ️  $Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error-Message {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Step {
    param([string]$Message)
    Write-Host "`n==> $Message" -ForegroundColor Yellow
}

# Step 1: Check Git Porcelain (clean working directory)
Write-Step "Checking Git working directory status..."
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Error-Message "Git working directory is not clean. Please commit or stash changes first."
    Write-Host "`nUncommitted changes:" -ForegroundColor Yellow
    Write-Host $gitStatus
    exit 1
}
Write-Success "Git working directory is clean"

# Step 2: Check for nuget-key.txt
Write-Step "Checking for NuGet API key file..."
$nugetKeyFile = "nuget-key.txt"
if (-not (Test-Path $nugetKeyFile)) {
    Write-Error-Message "NuGet API key file not found: $nugetKeyFile"
    Write-Info "Please create a file named 'nuget-key.txt' in the root directory containing your NuGet API key"
    exit 1
}
$nugetApiKey = Get-Content $nugetKeyFile -Raw
$nugetApiKey = $nugetApiKey.Trim()
if ([string]::IsNullOrWhiteSpace($nugetApiKey)) {
    Write-Error-Message "NuGet API key file is empty: $nugetKeyFile"
    exit 1
}
Write-Success "NuGet API key file found"

# Step 3: Run tests (unless skipped)
if (-not $SkipTests) {
    Write-Step "Running unit tests..."
    $testProject = "Cisco.Api.Test\Cisco.Api.Test.csproj"

    if (-not (Test-Path $testProject)) {
        Write-Error-Message "Test project not found: $testProject"
        exit 1
    }

    dotnet test $testProject --configuration Release --verbosity normal
    if ($LASTEXITCODE -ne 0) {
        Write-Error-Message "Unit tests failed"
        exit 1
    }
    Write-Success "All unit tests passed"
} else {
    Write-Info "Skipping unit tests (--skip-tests flag specified)"
}

# Step 4: Clean previous builds
Write-Step "Cleaning previous builds..."
dotnet clean --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "Clean failed"
    exit 1
}
Write-Success "Clean completed"

# Step 5: Build the project
Write-Step "Building project in Release mode..."
$projectFile = "Cisco.Api\Cisco.Api.csproj"
dotnet build $projectFile --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "Build failed"
    exit 1
}
Write-Success "Build completed successfully"

# Step 6: Pack the project with symbols (using modern embedded symbols format)
Write-Step "Packing NuGet package with symbols..."
dotnet pack $projectFile --configuration Release --no-build --include-symbols --include-source -p:SymbolPackageFormat=snupkg
if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "Pack failed"
    exit 1
}
Write-Success "Pack completed successfully"

# Step 7: Find the generated NuGet packages
Write-Step "Locating generated NuGet packages..."
$packagePath = Get-ChildItem -Path "Cisco.Api\bin\Release" -Filter "*.nupkg" -Recurse |
    Where-Object { $_.Name -notlike "*.symbols.nupkg" } |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if (-not $packagePath) {
    Write-Error-Message "No NuGet package found in Cisco.Api\bin\Release"
    exit 1
}

Write-Success "Found package: $($packagePath.Name)"
Write-Info "Package path: $($packagePath.FullName)"

# Find the symbols package
$symbolsPackagePath = Get-ChildItem -Path "Cisco.Api\bin\Release" -Filter "*.snupkg" -Recurse |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if ($symbolsPackagePath) {
    Write-Success "Found symbols package: $($symbolsPackagePath.Name)"
    Write-Info "Symbols package path: $($symbolsPackagePath.FullName)"
} else {
    Write-Info "No symbols package found (this is optional)"
}

# Step 8: Publish to NuGet
Write-Step "Publishing to NuGet.org..."
dotnet nuget push $packagePath.FullName --api-key $nugetApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "NuGet publish failed"
    exit 1
}
Write-Success "Package published successfully!"

# Step 9: Publish symbols package to NuGet
if ($symbolsPackagePath) {
    Write-Step "Publishing symbols package to NuGet.org..."
    dotnet nuget push $symbolsPackagePath.FullName --api-key $nugetApiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
    if ($LASTEXITCODE -ne 0) {
        Write-Error-Message "Symbols package publish failed"
        exit 1
    }
    Write-Success "Symbols package published successfully!"
}

Write-Host "`n🎉 All done! Package $($packagePath.Name) has been published to NuGet.org" -ForegroundColor Green
if ($symbolsPackagePath) {
    Write-Host "   Including symbols package for debugging support" -ForegroundColor Green
}
