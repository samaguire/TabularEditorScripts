# Tabular Editor 3 Scripts

For those interested in some cool (and practical) scripts for [Tabular Editor 3](https://tabulareditor.com/)! 😎

![image](https://user-images.githubusercontent.com/62320770/158300297-60ba262e-83e3-4575-ba90-6b847538ae3f.png)

## Installation Notes

### Quick Install

1. Opening this repo in Visual Studio Code will prompt to install the C# and Code Runner extensions if not already installed.
2. The dotnet script tool can be installed from the terminal using the command `dotnet tool install -g dotnet-script`
3. Update the Code Runner Executor Map setting for C# to `"csharp": "dotnet script"` from `"csharp": "cscript"`

### Visual Studio Extensions

To author C# scripts (and code) from Visual Studio Code, you need to install:

- The [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) extension.

To run C# scripts from Visual Studio Code (**the 'Macro Management' scripts in this case**) you need to install both:

- The [dotnet script](https://github.com/filipw/dotnet-script) tool, and
- the [Code Runner](https://marketplace.visualstudio.com/items?itemName=formulahendry.code-runner) extension.

## Macro Management Scripts

The scripts in the "Macro Mangement" folder of this repo are designed to be run from within Visual Studio Code.

If run from within Tabular Editor 3, there are a few requirements:

1. Only one instance of Tabular Editor 3 should be running, otherwise, any updates to the macros may not carry over into new sessions.
2. Tabular Editor 3 needs to be restarted for any updates to take effect.

## Warning

Though I do put much care into ensuring these scripts work. However, I offer no support if the use of these scripts either breaks TE3 or a model you are working. Use at your own risk!
