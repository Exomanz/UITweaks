# Contributing to the UITweaks Repository 
Are you a player and want to potentially see a feature added into this mod? A developer who found a bug and fixed it? There are multiple ways to contribute your ideas to the UITweaks Repository!

This project is licensed under the [MIT License](LICENSE). Read the license file to learn more.

## For Everyone - [Open an Issue](https://github.com/Exomanz/UITweaks/issues/new)
If you found a bug or have a feature request, please submit an issue. Fill out the form as fully as possible to make it easier to understand and reduce the back-and-forth trying to narrow down what is being discussed:

- **Title:** Short and to the point, include game and plugin versions if necessary.
- **Description:** As verbose as necessary to explain the issue or feature request. Screenshots and steps to reproduce a bug are always appreciated.
- **Attached Files:** If the issue pertains to a bug, please attach the `_latest.log` file from the Beat Saber directory and any additional screenshots/content you would like to include to aid in tracking down the cause of the problem. These can be videos, configuration files, etc. 

The more complete the issue form is, the faster the bug can be fixed and a patch can be released.

## For Developers - Clone UITweaks and [Create a Pull Request](https://github.com/Exomanz/UITweaks/compare)
If you are itching for a feature and want to implement it yourself, or simply have a code suggestion to help make this plugin better, submit a pull request! I would love to incorporate your code into this mod.

### Dependencies for Building
- .NET Framework v4.8.0
- BSIPA v4.3.4+
- SiraUtil v3.10.0+
- BeatSaberMarkupLanguage v1.11.2+
- _Heck v1.6.0+*

**If you are building the project without a local version of Heck installed, build in the `Debug` configuration. If Heck is present, build with `Heck-Debug` to enable features that integrate with Heck's API, such as `SettableSettings`. **You should not be building as `Release`.***

In order to build this project locally, clone the source code onto your machine, create the `UITweaks.csproj.user` file, and add your Beat Saber directory path to it.
This file should not be uploaded to GitHub, and is filtered out by the `.gitignore`.

**UITweaks.csproj.user**   
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- SET *YOUR OWN* BEAT SABER DIRECTORY HERE TO RESOLVE MOST DEPENDECY PATHS! -->
    <BeatSaberDir>C:\Program Files (x86)\Steam\steamapps\common\Beat Saber</BeatSaberDir>
  </PropertyGroup>
</Project>
```

### Adding New Dependencies
If you are going to add new dependencies, please ensure that the paths use `$(BeatSaberDir)` in the `UITweaks.csproj` file. This reduces potential merge conflicts and problems when new developers clone the code themselves with dependencies not resolving.

**UITweaks.csproj**   
```xml
...
  <Reference Include="Main.dll">
    <HintPath>$(BeatSaberDir)\Beat Saber_Data\Managed\Main.dll</HintPath>
  </Reference>
...
```
