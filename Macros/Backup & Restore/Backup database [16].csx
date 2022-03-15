#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "Microsoft.AnalysisServices.Core.dll"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using Microsoft.AnalysisServices;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***



BackupInfo backupInfo = new BackupInfo();

backupInfo.File = Model.Database.Name + ".abf";
backupInfo.AllowOverwrite = true;
backupInfo.ApplyCompression = true;

Model.Database.TOMDatabase.Backup(backupInfo);

ScriptHelper.Info("Script finished.");
