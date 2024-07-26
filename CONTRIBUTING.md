# Contributing to the UITweaks Repository 
Are you a player and want to potentially see a feature added into this mod? A developer who found a bug and fixed it? There are multiple ways to contribute your ideas to the UITweaks Repository!

This project is licensed under the [MIT License](LICENSE). Read the license file to learn more.

## [Open an Issue](https://github.com/Exomanz/UITweaks/issues/new)
If you found a bug or have a feature request, please submit an issue. Fill out the form as fully as possible to make it easier to understand and reduce the back-and-forth trying to narrow down what is being discussed.

## For Developers: Clone UITweaks and [Create a Pull Request](https://github.com/Exomanz/UITweaks/pulls)
This repository targets .NET Framework v4.8.0 and runs within the Unity Mono runtime. In order to build this project locally, clone the source code onto your machine, create the `UITweaks.csproj.user` file, and add your Beat Saber directory path to it.
This file should not be uploaded to GitHub, and is filtered out by the `.gitignore`.

### UITweaks.csproj.user:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- SET *YOUR OWN* BEAT SABER DIRECTORY HERE TO RESOLVE MOST DEPENDECY PATHS! -->
    <BeatSaberDir>C:\Program Files (x86)\Steam\steamapps\common\Beat Saber</BeatSaberDir>
  </PropertyGroup>
</Project>
```

## Adding New Dependencies
If you are going to add new dependencies, please ensure that the paths use `$(BeatSaberDir)` in `UITweaks.csproj`. This reduces potential merge conflicts and problems when new developers clone the code themselves with dependencies not resolving.

### UITweaks.csproj
```xml
...
  <Reference Include="Main.dll">
    <HintPath>$(BeatSaberDir)\Beat Saber_Data\Managed\Main.dll</HintPath>
  </Reference>
...
```