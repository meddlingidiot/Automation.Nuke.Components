# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased] - 1.0.46

### 🐛 Bug Fixes

- Fix `automation.nuke.builder` installation error by multi-targeting to .NET 8.0, 9.0, and 10.0, ensuring `DotnetToolSettings.xml` is available for all compatible SDKs.
- Multi-target `Automation.Nuke.Components` to .NET 8.0, 9.0, and 10.0 for broader project compatibility.

### 📝 Other Changes

- Remove redundant `new` modifier from `Main` method in `BuildFileGenerator` class. ([2d4c37c](../../commit/2d4c37c))
- Set minimum coverage threshold to 20 and clean up Velopack build class. ([9fc1435](../../commit/9fc1435))

## [1.0.45] - 2025-12-17

### 📝 Other Changes

- Replace `IGenerateCodeCoverage` with `IGenerateCoverageReport` and fix `IScanSecrets` to `IScanForSecrets` in build target definitions. ([1571827](../../commit/1571827))
- Remove redundant quotes in `git config` commands within `ConfigureGitIdentity` method. ([93ca342](../../commit/93ca342))
- Update Velopack publish directory: switch to temporary `.tmp/velopack-build` directory and ensure cleanup before publish ([3642cda](../../commit/3642cda))
- Refactor build process: remove commented connection string, clean up `IScanForSecrets`, and enhance modularity with additional task-specific interfaces. ([d4bca2e](../../commit/d4bca2e))
- Modularize build process: introduce new interfaces for test execution, Git tagging, release announcements, and code coverage generation; rename `IScanSecrets` to `IScanForSecrets`; refactor existing targets for improved maintainability and readability. ([348aded](../../commit/348aded))

## [1.0.44] - 2025-12-12

### 📝 Other Changes

- Enable connection-uri rule in Gitleaks and remove commented-out connection string. ([a0ddefb](../../commit/a0ddefb))

## [1.0.43] - 2025-12-12

### 📝 Other Changes

- Remove `--no-git` flag from Gitleaks command and expand documentation with non-interactive CI usage and custom component guide. ([cd551b8](../../commit/cd551b8))

## [1.0.42] - 2025-12-10

### 📝 Other Changes

- Add missing `using Automation.Nuke.Components.DefaultBuilds` directive in `BuildFileGenerator` ([cfee7f6](../../commit/cfee7f6))

## [1.0.41] - 2025-12-10

No changes recorded.

## [1.0.40] - 2025-12-10

### 📝 Other Changes

- Refactor `BuildFileGenerator`: replace inline interface declarations with `GetInterfacesForBuild` method, streamline build class generation, and clean up unused imports. ([04ae2a8](../../commit/04ae2a8))

## [1.0.39] - 2025-12-10

### 📝 Other Changes

- Refactor build components: modularize clean, restore, and compile tasks with new `IClean`, `IRestore`, and `ICompile` interfaces; update changelog handling and enhance Azure Pipelines configurations. ([37f4ef2](../../commit/37f4ef2))

## [1.0.38] - 2025-12-09

### 📝 Other Changes

- Update `SetupCommand` to include `.gitignore` updates with Nuke and Rider entries, add `UpdateGitIgnoreAsync` in `NuGetPackageInstaller`, and adjust `.gitignore` content. ([df8761f](../../commit/df8761f))

## [1.0.37] - 2025-12-08

### 📝 Other Changes

- Update `BuildFileGenerator`: use `new` modifier in `Main` method and remove unused post-release target logic. ([98f3417](../../commit/98f3417))

## [1.0.36] - 2025-12-08

### 📝 Other Changes

- Switch to async output handling in `NuGetPackageInstaller` to prevent buffer deadlock, add timeout-based process termination, and include launch settings file for debugging. ([35475b2](../../commit/35475b2))

## [1.0.35] - 2025-12-08

### 📝 Other Changes

- Refactor Velopack icon selection: add `.ico` file scanning, custom path option, and improved prompt logic ([bdbe57d](../../commit/bdbe57d))

## [1.0.34] - 2025-12-08

### 📝 Other Changes

- Introduce `ProjectDiscovery` service, refactor Velopack prompt logic, and clean up unused package references ([4818d83](../../commit/4818d83))

## [1.0.33] - 2025-12-08

### 📝 Other Changes

- Update `Automation.Nuke.Components` package, switch to embedding `DefaultRootItems` as resources, and improve NuGet package handling logic ([055effb](../../commit/055effb))

## [1.0.32] - 2025-12-08

### 📝 Other Changes

- Enhance `BuildFileGenerator` to support additional interfaces (`IHasSolution`, `IHasConfiguration`), improve target generation logic, and update build schema with `MinCoverageThreshold`. ([b49732f](../../commit/b49732f))
- Update `build` project by adding `Automation.Nuke.Components` package, reorganizing imports, and enabling test support in `Build` class ([4a8f377](../../commit/4a8f377))

## [1.0.31] - 2025-12-07

### 📝 Other Changes

- Introduce `Automation.Nuke.Builder` project for simplifying Nuke pipeline setups, remove unused NuGet handling logic, and adjust solution structure. ([f7ab44f](../../commit/f7ab44f))

## [1.0.30] - 2025-12-07

### 📝 Other Changes

- Comment out `.EnableNoBuild()` in `IPackage` to revert potential fix. ([ae5661f](../../commit/ae5661f))

## [1.0.29] - 2025-12-07

### 📝 Other Changes

- Uncomment `.EnableNoBuild()` in `IPackage` to test potential fix. ([4b83977](../../commit/4b83977))

## [1.0.28] - 2025-12-07

### 📝 Other Changes

- Comment out `.EnableNoBuild()` in `IPackage` to test potential fix. ([711b85e](../../commit/711b85e))

## [1.0.27] - 2025-12-07

### 📝 Other Changes

- Migrate DefaultBuilds to AzurePipelinesBuild base class and introduce Azure-specific configuration. ([9a2ee4a](../../commit/9a2ee4a))

## [1.0.26] - 2025-12-07

### 📝 Other Changes

- Add Gitleaks configuration and implement ForceTagRelease parameter ([c01a7a6](../../commit/c01a7a6))

## [1.0.25] - 2025-12-07

No changes recorded.

## [1.0.2] - 2025-12-07

### 📝 Other Changes

- Remove `Configuration` class, adjust dependencies, and add `SharpZipLib` package to the build project ([728bd59](../../commit/728bd59))

## [1.0.1] - 2025-12-07

### 📝 Other Changes

- Add entries to .gitignore for Nuke temporary files and IDE configuration ([b241b28](../../commit/b241b28))
- Add entries to .gitignore for JetBrains Rider local history and Nuke temporary files ([129fe13](../../commit/129fe13))
- Add .store/ to .gitignore to exclude store files from version control ([caad29f](../../commit/caad29f))
- Add .gitignore file to exclude Visual Studio and build artifacts ([491afc1](../../commit/491afc1))
- Add .gitignore files and include `IVelopack` in PackageBuild steps ([3d42371](../../commit/3d42371))
- Extend build schema to support Velopack tasks and Azure Blob configuration ([079a14f](../../commit/079a14f))
- Upgrade build components with enhanced tagging, release handling, and project validation. ([5a72769](../../commit/5a72769))
- Add GitVersion.Tool package download to the build project ([d9d303e](../../commit/d9d303e))
- Add GitVersion.Tool installation and configure dotnet-tools.json for build automation ([fa743a3](../../commit/fa743a3))
- Integrate Automation.Nuke.Components and refactor Build class to extend PackageBuild ([d84c3d5](../../commit/d84c3d5))
- Update target framework to .NET 10 in build configuration ([88dc2a5](../../commit/88dc2a5))
- Initialize NUKE build system configuration and schema ([db72072](../../commit/db72072))
- Mark `Main` method as `new` in `Build.cs` ([ab105d1](../../commit/ab105d1))
- Set up CI with Azure Pipelines ([9d5b9f3](../../commit/9d5b9f3))
- Add cross-platform build scripts and project configurations ([9e646bb](../../commit/9e646bb))

## [1.0.0] - 2025-12-06

No changes recorded.

## [0.0.1] - 2025-12-06

### 📝 Other Changes

- .gitignore (VisualStudio) files ([8e7c231](../../commit/8e7c231))

