# Tabular Editor Scripting in Visual Studio Code

For those interested in writing scripts for Tabular Editor 2/3 from within Visual Studio Code; giving you the added benefit of source control for your Tabular Editor scripts as well as IntelliSense if you don't use Tabular Editor 3 👏.

![image](https://user-images.githubusercontent.com/62320770/210715420-487a4a8a-6b2f-47d1-84b3-d511b2778060.png)

## Requirements

The below software needs to be installed. (Portable versions of Tabular Editor are not compatible.)

### Required

- [Visual Studio Code](https://code.visualstudio.com/)
- [.NET SDK 6.0 or 7.0](https://dotnet.microsoft.com/en-us/download)
- [Tabular Editor 2](https://github.com/TabularEditor/TabularEditor) (free, open-source version)

### Optional

- [Tabular Editor 3](https://tabulareditor.com/)

## Installation

### Quick Install

1. Opening this repo in Visual Studio Code will prompt to install the C# and Code Runner extensions if not already installed.
2. The dotnet script tool can be installed from the terminal using the command `dotnet tool install -g dotnet-script`
3. Update the Code Runner Executor Map setting for C# to `"csharp": "dotnet script --isolated-load-context"` from `"csharp": "cscript"`

### Visual Studio Extensions

To author C# scripts (and code) from Visual Studio Code, you need to install:

- The [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) extension.

To run C# scripts from Visual Studio Code (**the Management scripts in this case**) you need to install:

- The [dotnet script](https://github.com/filipw/dotnet-script) extension, and
- optionally, the [Code Runner](https://marketplace.visualstudio.com/items?itemName=formulahendry.code-runner) extension (but highly recommended).

### dotnet script

If you get the "`Tool 'dotnet-script' failed to install`" error when executing the install command from terminal, try updating the package source in your NuGet config file as per [this](https://stackoverflow.com/a/68140757) post on Stack Overflow.

## Macro Management Scripts

The scripts in the "Management" folder of this repo are designed to be run from within Visual Studio Code without Tabular Editor running.

- Macros Export from TE.csx
- Macros Import into TE.csx
- Settings Backup.csx
- Settings Restore.csx
- TE2 Script Compiler Update.csx

These scripts are compatible with either Tabular Editor 3 or Tabular Editor 2. (With the exception of the 'TE2 Script Compiler Update.csx' script which applies to Tabular Editor 2 only.)

The default behaviour is to export/import into/from Tabular Editor 3 if it is detected, otherwise, Tabular Editor 2 will be used. This behaviour can be controlled by setting the variable 'TE3overTE2' to 'true' or 'false' in the export/import scripts. If the variable 'TE3overTE2' is set to 'false', then Tabular Editor 2 will be exported/imported into/from even if Tabular Editor 3 is installed.

### Run & Debug

If you intend to to use the native debugger to run the Macro Management Scripts then you will also need to execute the command `dotnet script init` to reset the environemnt before running the debugger.

## Warning

Use at your own risk! Although I put much care into ensuring these scripts work, I offer no support if the use of these scripts breaks either Tabular Editor or a model you are working on.
