#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "Microsoft.AnalysisServices.Core.dll"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***
using Microsoft.AnalysisServices;

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***



BackupInfo backupInfo = new BackupInfo();

backupInfo.File = Model.Database.Name + ".abf";
backupInfo.AllowOverwrite = true;
backupInfo.ApplyCompression = true;

Model.Database.TOMDatabase.Backup(backupInfo);

Info("Script finished.");
