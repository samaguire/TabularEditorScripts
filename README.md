# Tabular Editor 3 Scripts

For those interested in some cool (and practical) scripts for [Tabular Editor 3](https://tabulareditor.com/)! 😎

![image](https://user-images.githubusercontent.com/62320770/158300297-60ba262e-83e3-4575-ba90-6b847538ae3f.png)

## Installation

Opening this repo in Visual Studio Code prompt to install the C# and Code Runner extensions if not already installed.



### Quick steps

1. Install the latest [Visual Studio Code](https://code.visualstudio.com/).

2. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0). (This will be installed by Tabular Editor 3.3 or higher.)

3. Install the .NET Interactive Notebooks extension from the [marketplace](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode).

4. Download notebooks from the latest [release](https://github.com/samaguire/PowerBINotebooks/releases/latest) and open in Visual Studio Code.

# Installation Notes

## Visual Studio Extensions

To author C# scripts (and code) from Visual Studio Code, you need to install:

- The [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) extension.

To run C# scripts (**the 'Macro Management' scripts in this case**) from Visual Studio Code, you need to install both:

- The [Code Runner](https://marketplace.visualstudio.com/items?itemName=formulahendry.code-runner) extension, and
- [scriptcs](http://scriptcs.net/) as per the Code Runner notes.
- [dotnet script](https://github.com/filipw/dotnet-script)

## Macro Management Scripts

The scripts in the "Macro Mangement" folder of this repo are designed to be run from within Visual Studio Code.

If run from within Tabular Editor 3, there are a few requirements:

1. Only one instance of Tabular Editor 3 should be running, otherwise, any updates to the macros <u>may</u> not carry over into new sessions.
2. Tabular Editor 3 needs to be restarted for any updates to take effect.

# Warning

Though I do put much care into ensuring these scripts work. However, I offer no support if the use of these scripts either breaks TE3 or a model you are working. Use at your own risk!
