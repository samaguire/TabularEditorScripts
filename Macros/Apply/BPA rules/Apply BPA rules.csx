#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

CustomAction("Macros\\BPA rules\\Download rules");
CustomAction("Macros\\BPA rules\\Add long length column annotations");
CustomAction("Macros\\BPA rules\\Add split datetime annotations");
CustomAction("Macros\\BPA rules\\Add VertiPaq annotations");

Info("All scripts finished.");
