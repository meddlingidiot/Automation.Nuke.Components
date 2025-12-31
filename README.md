# Automation.Nuke.Components

A comprehensive Nuke build automation framework designed for enterprise-scale .NET projects. This solution provides reusable build components, pre-configured build templates, and an interactive setup tool to streamline CI/CD pipeline creation across multiple projects.

[![Build](https://github.com/meddlingidiot/Automation.Nuke.Components/actions/workflows/build.yml/badge.svg)](https://github.com/meddlingidiot/Automation.Nuke.Components/actions/workflows/build.yml)
[![codecov](https://codecov.io/gh/meddlingidiot/Automation.Nuke.Components/branch/main/graph/badge.svg)](https://codecov.io/gh/meddlingidiot/Automation.Nuke.Components)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/meddlingidiot/Automation.Nuke.Components)](https://github.com/meddlingidiot/Automation.Nuke.Components/releases)
[![License](https://img.shields.io/github/license/meddlingidiot/Automation.Nuke.Components)](LICENSE)

## 📦 Projects

### Automation.Nuke.Components
A library of modular, reusable Nuke build components that can be composed to create sophisticated build pipelines.

**Key Features:**
- **Modular Components**: Mix and match build steps (compile, test, package, deploy)
- **Pre-configured Templates**: Five ready-to-use build configurations
- **Parameter Management**: Strongly-typed configuration with sensible defaults
- **Multi-targeting**: Supports .NET 8.0, 9.0, and 10.0

### Automation.Nuke.Builder
A global .NET tool (`aftrnuke`) that provides an interactive setup experience for configuring Nuke builds across projects.

**Key Features:**
- **Interactive Setup**: Guided prompts using Spectre.Console
- **Automated Tooling**: Auto-installs GitVersion, ReportGenerator, and Gitleaks configs
- **Convention-based**: Automatically configures standard project structures
- **Non-interactive Mode**: CI-friendly for automated deployments

## 🚀 Quick Start

### Installation

#### From GitHub Packages

First, authenticate to GitHub Packages (required once):

```bash
dotnet nuget add source --username YOUR_GITHUB_USERNAME --password YOUR_GITHUB_PAT --store-password-in-clear-text --name github "https://nuget.pkg.github.com/meddlingidiot/index.json"
```

Then install the builder tool globally:

```bash
dotnet tool install --global Automation.Nuke.Builder --add-source github
```

#### From Local Build

Or build and install locally:

```bash
cd Automation.Nuke.Builder
dotnet pack
dotnet tool install --global --add-source ./bin/Debug Automation.Nuke.Builder
```

### Setup Your First Build

Navigate to your project root and run:

```bash
aftrnuke setup
```

Follow the interactive prompts to:
1. Select your build type (Compile, Test, Package, Velopack, or Combined)
2. Configure quality gates (warnings, secrets, code coverage)
3. Set up Velopack deployment (if applicable)

### Run Your Build

```bash
nuke
```

Or run specific targets:

```bash
nuke Test
nuke Package
nuke ReleasePackage
```

## 📋 Build Templates

### CompileBuild
**Purpose**: Basic compilation with secret scanning

**Includes:**
- ✅ Version display (`IShowVersion`)
- ✅ Clean artifacts (`IClean`)
- ✅ Restore dependencies (`IRestore`)
- ✅ Compile solution (`ICompile`)
- ✅ Scan for secrets (`IScanForSecrets`)

**Use Case**: Simple libraries or projects needing only compilation verification

**Example Build.cs:**
```csharp
public class Build : AzurePipelinesBuild, IShowVersion, IClean, IRestore,
    ICompile, IScanForSecrets
{
    public static int Main() => Execute<Build>(x => ((ICompile)x).Compile);
}
```

---

### TestBuild
**Purpose**: Full testing pipeline with code coverage

**Includes:**
- All CompileBuild features
- ✅ Unit tests (`IRunUnitTests`)
- ✅ Integration tests (`IRunIntegrationTests`)
- ✅ Code coverage reports (`IGenerateCoverageReport`)
- ✅ Test orchestration (`ITest`)

**Use Case**: Libraries with comprehensive test suites

**Example Build.cs:**
```csharp
public class Build : AzurePipelinesBuild, IShowVersion, IClean, IRestore,
    ICompile, IScanForSecrets, IRunUnitTests, IRunIntegrationTests,
    IGenerateCoverageReport, ITest
{
    public static int Main() => Execute<Build>(x => ((ITest)x).Test);

    int IHasTests.MinCoverageThreshold => 80;
    bool IHasTests.BreakBuildOnWarnings => true;
}
```

---

### PackageBuild
**Purpose**: NuGet package publishing pipeline

**Includes:**
- All TestBuild features
- ✅ Changelog updates (`IUpdateChangelog`)
- ✅ NuGet packaging (`IPackage`)
- ✅ Git tagging (`ITagRelease`)
- ✅ Release announcements (`IAnnounceRelease`)

**Use Case**: Libraries published to NuGet or GitHub Packages

**Example Build.cs:**
```csharp
public class Build : AzurePipelinesBuild, IShowVersion, IClean, IRestore,
    ICompile, IScanForSecrets, IRunUnitTests, IRunIntegrationTests,
    IGenerateCoverageReport, ITest, IUpdateChangelog, IPackage,
    ITagRelease, IAnnounceRelease
{
    public static int Main() => Execute<Build>(x => ((IPackage)x).ReleasePackage);

    // Required for GitHub Packages
    string IHasGitHubPackages.GitHubOwner => "your-github-username";

    int IHasTests.MinCoverageThreshold => 80;
}
```

---

### VelopackBuild
**Purpose**: Desktop application deployment with Velopack

**Includes:**
- All TestBuild features
- ✅ Velopack installer creation (`IVelopack`)
- ✅ Azure Blob deployment
- ✅ Git tagging (`ITagRelease`)

**Use Case**: Desktop applications using Velopack for auto-updates

**Example Build.cs:**
```csharp
public class Build : AzurePipelinesBuild, IShowVersion, IClean, IRestore,
    ICompile, IScanForSecrets, IRunUnitTests, IRunIntegrationTests,
    IGenerateCoverageReport, ITest, IVelopack, ITagRelease
{
    public static int Main() => Execute<Build>(x => ((IVelopack)x).ReleaseVelopack);

    string IHasVelopack.VelopackProjectName => "MyDesktopApp";
    string IHasVelopack.VelopackIconPath => RootDirectory / "assets" / "icon.ico";
    string IHasVelopack.AzureBlobAccount => "mystorageaccount";
    string IHasVelopack.VelopackBlobContainer => "releases";
}
```

---

### PackageAndVelopackBuild
**Purpose**: Complete pipeline for hybrid projects

**Includes:**
- All TestBuild features
- ✅ NuGet packaging (`IPackage`)
- ✅ Velopack deployment (`IVelopack`)
- ✅ Git tagging (`ITagRelease`)
- ✅ Release announcements (`IAnnounceRelease`)

**Use Case**: Complex projects distributing both libraries and desktop applications

---

## 🧩 Available Components

### Build Steps
| Component | Description | Key Targets |
|-----------|-------------|-------------|
| `IShowVersion` | Display GitVersion info | `ShowVersion` |
| `IClean` | Clean build artifacts | `Clean` |
| `IRestore` | Restore NuGet packages | `Restore` |
| `ICompile` | Compile solution | `Compile` |
| `IScanForSecrets` | Gitleaks secret scanning | `ScanForSecrets` |
| `IRunUnitTests` | Execute unit tests | `RunUnitTests` |
| `IRunIntegrationTests` | Execute integration tests | `RunIntegrationTests` |
| `IGenerateCoverageReport` | Generate coverage reports | `GenerateCoverageReport` |
| `ITest` | Orchestrate all tests | `Test` |
| `IUpdateChangelog` | Update CHANGELOG.md | `UpdateChangelog` |
| `IPackage` | Create NuGet packages | `Package`, `ReleasePackage` |
| `IVelopack` | Create Velopack installers | `Velopack`, `ReleaseVelopack` |
| `ITagRelease` | Create Git tags | `TagRelease` |
| `IAnnounceRelease` | Announce releases | `AnnounceRelease` |
| `IPostRelease` | Post-release cleanup | `PostRelease` |

### Configuration Parameters
| Interface | Parameters | Purpose |
|-----------|------------|---------|
| `IHasSolution` | `Solution` | Solution file reference |
| `IHasConfiguration` | `Configuration` | Build configuration (Debug/Release) |
| `IHasGitVersion` | `GitVersion` | GitVersion integration |
| `IHasArtifacts` | `ArtifactsDirectory`, etc. | Output directories |
| `IHasTests` | `MinCoverageThreshold`, `BreakBuildOnWarnings` | Test quality gates |
| `IHasGitHubPackages` | `GitHubOwner`, `GitHubToken` | GitHub Packages auth |
| `IHasAzureDevOpsFeeds` | `ProductionFeedId`, `PrereleaseFeedId` | Azure Artifacts |
| `IHasVelopack` | `VelopackProjectName`, `AzureBlobAccount` | Velopack config |
| `IDoTag` | `ForceTagRelease` | Git tagging behavior |

## ⚙️ Configuration

### Overriding Parameters

Override interface properties in your `Build.cs`:

```csharp
public class Build : PackageBuild
{
    // Override minimum code coverage
    int IHasTests.MinCoverageThreshold => 85;

    // Override warning behavior
    bool IHasTests.BreakBuildOnWarnings => true;

    // Set GitHub owner for package publishing
    string IHasGitHubPackages.GitHubOwner => "meddlingidiot";

    // Customize artifact directory
    AbsolutePath IHasArtifacts.ArtifactsDirectory => RootDirectory / "dist";
}
```

### Command-Line Parameters

Pass parameters at runtime:

```bash
nuke --configuration Release
nuke --min-coverage-threshold 90
nuke --github-owner myorganization
nuke --force-tag-release
```

### Environment Variables

Common environment variables:

| Variable | Purpose | Used By |
|----------|---------|---------|
| `GITHUB_TOKEN` | GitHub authentication | `IHasGitHubPackages` |
| `AZURE_BLOB_SAS_TOKEN` | Azure Blob access | `IHasVelopack` |

## 🛠️ Custom Components

Create your own reusable components:

### 1. Define Your Component Interface

```csharp
// build/Components/IMyLint.cs
using Nuke.Common;
using Nuke.Common.IO;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace MyProject.Build.Components;

public interface IMyLint : INukeBuild, IHasSolution
{
    Target MyLint => _ => _
        .Description("Run custom linting")
        .DependsOn<ICompile>()
        .Executes(() =>
        {
            DotNet($"format {Solution} --verify-no-changes");
        });
}
```

### 2. Wire Into Your Build

```csharp
public class Build : PackageBuild, IMyLint
{
    public static int Main() => Execute<Build>(x => ((IPackage)x).ReleasePackage);

    // Custom lint will run as part of Test dependencies
    Target ITest.Test => _ => _
        .DependsOn(((IMyLint)this).MyLint)
        .DependsOn(((ITestExecution)this).TestExecution);
}
```

## 📁 Generated Project Structure

After running `aftrnuke setup`, your project will have:

```
MyProject/
├── build/
│   ├── Build.cs              # Generated build script
│   ├── _build.csproj         # Build project (targets net10.0)
│   └── .nuke                 # Nuke configuration
├── .config/
│   └── dotnet-tools.json     # Local tool manifest (GitVersion)
├── .github/
│   └── workflows/
│       └── build.yml         # (Optional) GitHub Actions workflow
├── artifacts/                # Build outputs
│   ├── packages/            # NuGet packages
│   ├── velopack/            # Velopack installers
│   ├── test-results/        # Test results
│   └── coverage-report/     # Coverage HTML reports
├── .gitleaks.toml            # Secret scanning configuration
├── nuget.config              # NuGet source configuration
├── GitVersion.yml            # GitVersion configuration
├── build.cmd                 # Windows build entry point
├── build.sh                  # Linux/Mac build entry point
└── build.ps1                 # PowerShell build entry point
```

## 🔧 Requirements

### For Using the Builder Tool
- .NET SDK 8.0 or higher

### For Running Generated Builds
- .NET SDK 10.0
- Git (for version control features)
- GitVersion.Tool (installed automatically by setup)
- Gitleaks (for secret scanning - [install separately](https://github.com/gitleaks/gitleaks))

### Optional Dependencies
- Azure CLI (for Azure Blob deployments)
- Velopack CLI (installed automatically when needed)

## 🚦 CI/CD Integration

### Azure Pipelines

The generated `azure-pipelines.yml` includes:

```yaml
trigger:
  branches:
    include:
      - main
      - develop

pool:
  vmImage: 'windows-latest'

steps:
  - task: UseDotNet@2
    inputs:
      version: '10.0.x'

  - script: dotnet tool restore
    displayName: 'Restore Tools'

  - script: .\build.cmd
    displayName: 'Run Build'
```

### GitHub Actions

The generated `build.yml` includes:

```yaml
name: Build

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
        with:
          fetch-depth: 0

      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'

      - run: dotnet tool restore
      - run: ./build.cmd
```

## 📝 Secrets Management

### Required Secrets for GitHub Packages

Set these in your CI/CD environment:

| Secret | Purpose | Scope |
|--------|---------|-------|
| `GITHUB_TOKEN` | Push packages to GitHub Packages | Repository |
| `AZURE_BLOB_SAS_TOKEN` | Upload Velopack releases | Pipeline |

### Local Development

For local testing, use command-line parameters:

```bash
nuke ReleasePackage --github-token "ghp_YOUR_TOKEN"
nuke ReleaseVelopack --azure-blob-sas-token "sv=2022-11-02&..."
```

## 🔍 Troubleshooting

### "GitHubOwner parameter is required"

Add to your `Build.cs`:
```csharp
string IHasGitHubPackages.GitHubOwner => "your-username-or-org";
```

Or pass via command line:
```bash
nuke --github-owner your-username-or-org
```

### "Nuke not found"

Ensure .NET global tools are in your PATH:

**Windows:**
```powershell
$env:PATH += ";$env:USERPROFILE\.dotnet\tools"
```

**Linux/Mac:**
```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

### Build fails targeting net10.0

Install the .NET 10 SDK:
```bash
winget install Microsoft.DotNet.SDK.10
# or
brew install --cask dotnet-sdk
```

### Gitleaks not found

Install Gitleaks:

**Windows:**
```powershell
winget install gitleaks
```

**Mac:**
```bash
brew install gitleaks
```

**Linux:**
```bash
# Download from https://github.com/gitleaks/gitleaks/releases
```

### Package not found after setup

Ensure your NuGet feed is configured:
```bash
dotnet nuget add source https://your-feed-url --name YourFeed
```

### Code coverage badge not showing

You have several options for code coverage badges:

#### **Option A: GitHub Actions Dynamic Badge** ⭐ (No external service required!)

1. **Create a GitHub Gist**:
   - Go to https://gist.github.com/
   - Create a new **public** gist named `coverage-badge.json` with content: `{}`
   - Copy the Gist ID from URL (e.g., `abc123def456`)

2. **Create Personal Access Token**:
   - Go to https://github.com/settings/tokens/new
   - Name: `Gist Badge Token`
   - Select scope: **gist** only
   - Generate and copy token

3. **Add Secrets to GitHub**:
   - Go to repo Settings → Secrets → Actions
   - Add `GIST_SECRET` with the token value

4. **Update workflow**:
   - Edit `.github/workflows/build.yml` line 142
   - Replace `YOUR_GIST_ID_HERE` with your actual Gist ID

5. **Update README badge**:
   ```markdown
   [![Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/YOUR_USERNAME/YOUR_GIST_ID/raw/coverage-badge.json)](https://github.com/meddlingidiot/Automation.Nuke.Components/actions)
   ```

#### **Option B: Codecov** (Popular hosted service)

1. Sign up at https://codecov.io/ and connect repository
2. Add `CODECOV_TOKEN` to GitHub Secrets
3. Badge already in README works automatically

#### **Option C: Coveralls** (Alternative hosted service)

1. Sign up at https://coveralls.io/
2. Add `COVERALLS_TOKEN` to GitHub Secrets
3. Update workflow action and badge URL

**Recommended**: Option A - no external service, fully self-hosted!

## 🤝 Contributing

Contributions are welcome! This framework is designed to be extended.

### Adding a New Component

1. Create interface in `Automation.Nuke.Components/Components/`
2. Add any required parameters in `Parameters/`
3. Update relevant `DefaultBuilds/` templates
4. Test with `aftrnuke setup`
5. Update documentation

### Adding a New Build Template

1. Create class in `Automation.Nuke.Components/DefaultBuilds/`
2. Compose from existing components
3. Add to `BuildFileGenerator.cs` in the Builder project
4. Update prompts in `SetupCommand.cs`

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🔗 Links

- **Repository**: https://github.com/meddlingidiot/Automation.Nuke.Components
- **GitHub Packages**: https://github.com/meddlingidiot/Automation.Nuke.Components/packages
- **Releases**: https://github.com/meddlingidiot/Automation.Nuke.Components/releases
- **Nuke Build**: https://nuke.build/
- **Issue Tracker**: https://github.com/meddlingidiot/Automation.Nuke.Components/issues

## 💡 Examples

For real-world examples, see:
- This repository's own `build/Build.cs`
- [Example projects](https://github.com/meddlingidiot/Automation.Nuke.Components/tree/main/examples) (coming soon)

---

**Built with ❤️ using [Nuke Build](https://nuke.build/)**
