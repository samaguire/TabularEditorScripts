#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

ScriptHelper.CustomAction("Macros\\BPA rules\\Download rules");
ScriptHelper.CustomAction("Macros\\BPA rules\\Add long length column annotations");
ScriptHelper.CustomAction("Macros\\BPA rules\\Add split datetime annotations");
ScriptHelper.CustomAction("Macros\\BPA rules\\Add VertiPaq annotations");

ScriptHelper.Info("All scripts finished.");
