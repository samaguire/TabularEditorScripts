# Tabular Editor Scripting in Visual Studio Code

For those interested in writing scripts for Tabular Editor 2/3 from with in Visual Studio Code.

<'image'>

## Requirements

The below software needs to be installed. Portable versions are not compatible.

### Required

- [Visual Studio Code](https://code.visualstudio.com/)
- [Tabular Editor 2](https://github.com/TabularEditor/TabularEditor) (free, open-source version)

### Optional

- [Tabular Editor 3](https://tabulareditor.com/)

## Installation Notes

### Quick Install

1. Opening this repo in Visual Studio Code will prompt to install the C# and Code Runner extensions if not already installed.
2. The dotnet script tool can be installed from the terminal using the command `dotnet tool install -g dotnet-script`
3. Update the Code Runner Executor Map setting for C# to `"csharp": "dotnet script --isolated-load-context"` from `"csharp": "cscript"`

### Visual Studio Extensions

To author C# scripts (and code) from Visual Studio Code, you need to install:

- The [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) extension.

To run C# scripts from Visual Studio Code (**the Management scripts in this case**) you need to install both:

- The [dotnet script](https://github.com/filipw/dotnet-script) tool, and
- the [Code Runner](https://marketplace.visualstudio.com/items?itemName=formulahendry.code-runner) extension.

### dotnet script

If you get the "`Tool 'dotnet-script' failed to install`" error when executing the install command from terminal, try updating the package source in your NuGet config file as per [this](https://stackoverflow.com/a/68140757) post on Stack Overflow.

## Macro Management Scripts

The scripts in the "Mangement" folder of this repo are designed to be run from within Visual Studio Code without Tabular Editor running.

- Macros Export from TE.csx
- Macros Import into TE.csx
- Settings Backup.csx
- Settings Restore.csx
- TE Script Compiler Update.csx (for updating the script compiler in Tabular Editor 2, not required for Tabular Editor 3)

These scripts are are compatible with either Tabular Editor 3 or Tabular Editor 2. (With the exception of the 'TE Script Compiler Update.csx' script which doesn't apply to Tabular Editor 3.)

The default behaviour is to export/import into/from Tabular Editor 3 if it is detected, otherwise, Tabular Editor 2 will be used. This behaviour can be controlled by setting the variable 'TE3overTE2' to 'true' or 'false' in the export/import scripts. If the variable 'TE3overTE2' is set to 'false', then Tabular Editor 2 will be exported/imported into/from even if Tabular Editor 3 is installed.

## Warning

Though I do put much care into ensuring these scripts work. However, I offer no support if the use of these scripts either breaks TE3 or a model you are working. Use at your own risk
