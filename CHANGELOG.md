# Changelog

All notable changes to this project will be documented in this file.

## [1.0.16] - 2026-02-06

### 🔧 Chores

- chore: Bump `System.CommandLine` to v2.0.2 and remove Velopack artifact upload step from workflows ([34f0fec](../../commit/34f0fec))

## [1.0.15] - 2026-01-19

### 🔧 Chores

- chore: Update Velopack directory path and validate `VelopackProjectName` in build steps ([08340ff](../../commit/08340ff))

## [1.0.14] - 2026-01-01

### 🔧 Chores

- chore: Remove `IVelopack` interface and related release step from build process ([21227c2](../../commit/21227c2))
- chore: Update test package dependencies to latest versions and adjust metadata ([ae93b74](../../commit/ae93b74))
- chore: Add `gitleaks.toml` configuration file for secret scanning ([ced1fb3](../../commit/ced1fb3))

## [1.0.13] - 2025-12-31

### 📝 Other Changes

- Fix resource path handling in NuGet installer; restore hidden directories like `.github` and improve debug logging ([07176ba](../../commit/07176ba))

## [1.0.12] - 2025-12-30

### 📝 Other Changes

- Update GitHub workflow to use `GH_PACKAGES_TOKEN` for authentication and adjust related environment variables ([309a6b2](../../commit/309a6b2))

## [1.0.11] - 2025-12-30

### 📝 Other Changes

- Rename `ToolCommandName` to `autonuke` and update branding/references to "MeddlingIdiot". ([ca48128](../../commit/ca48128))

## [1.0.10] - 2025-12-30

### 📝 Other Changes

- Update GitHub workflow authentication to use `GITHUB_TOKEN` and comment out coverage-related steps. ([605060b](../../commit/605060b))

## [1.0.9] - 2025-12-30

### 📝 Other Changes

- Add README.md with detailed documentation and update workflows to generate a coverage badge. ([52f9d6c](../../commit/52f9d6c))

## [1.0.8] - 2025-12-27

### 📝 Other Changes

- Remove unused AFTR package source URLs and add `nuget.config` to solution items. ([7b2c1a2](../../commit/7b2c1a2))

## [1.0.7] - 2025-12-27

### 📝 Other Changes

- Ensure Velopack CLI is installed/updated before executing commands in `IVelopack`. ([0de8565](../../commit/0de8565))
- Remove `azure-pipelines.yml` from solution items ([95e46fc](../../commit/95e46fc))

## [1.0.6] - 2025-12-27

### 📝 Other Changes

- Remove outdated SAS token URL comment from `IHasVelopack`. ([039ddb1](../../commit/039ddb1))
- Update `IHasVelopack` with new Azure Blob storage details and SAS token expiration ([e8ac85d](../../commit/e8ac85d))

## [1.0.5] - 2025-12-27

### 📝 Other Changes

- Fix packaging of `DotnetToolSettings.xml` by changing item type to `None` with `Pack=true`. ([70b9b89](../../commit/70b9b89))

## [1.0.4] - 2025-12-27

### 📝 Other Changes

- Include `DotnetToolSettings.xml` in the package to define CLI commands for `Automation.Nuke.Builder`. ([54c80e0](../../commit/54c80e0))

## [1.0.3] - 2025-12-27

### 📝 Other Changes

- Update `Automation.Nuke.Components` to target .NET 10 only, removing multi-targeting support. ([de995c7](../../commit/de995c7))
- Upgrade to .NET 10 and update repository URL to GitHub for `Automation.Nuke.Builder`. ([a14f094](../../commit/a14f094))
- Multi-target projects to .NET 8.0, 9.0, and 10.0 for improved compatibility and resolve installation issues with `automation.nuke.builder`. ([9a1cddb](../../commit/9a1cddb))

## [1.0.2] - 2025-12-26

### 📝 Other Changes

- Migrate build system to GitHub Actions with .NET 10 support ([d23e7ec](../../commit/d23e7ec))

## [1.0.1] - 2025-12-26

### 📝 Other Changes

- Allow test results publishing step to continue on error in GitHub Actions workflow ([a21aca8](../../commit/a21aca8))

## [1.0.0] - 2025-12-26

No changes recorded.

## [0.0.1] - 2025-12-26

### 📝 Other Changes

- Add GitHub repository owner parameter to Nuke build commands and update runner to `windows-latest` ([f0ba037](../../commit/f0ba037))
- Add support for publishing NuGet packages to GitHub Packages ([8b57da5](../../commit/8b57da5))
- Change build runner to windows-latest ([70a61f9](../../commit/70a61f9))
- Add GitHub Actions CI workflow and unit tests for build automation ([ef3c613](../../commit/ef3c613))

