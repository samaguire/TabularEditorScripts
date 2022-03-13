#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "Microsoft.AnalysisServices.Core.dll"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***
using System.Diagnostics;

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

// https://github.com/TabularEditor/TabularEditor3/issues/249#issuecomment-939848828



var connectionInfo = Model.Database.TOMDatabase.Server.ConnectionInfo;
var server = "";
var database = "";

if (connectionInfo.Port == null)
{

    server = connectionInfo.Server;
    database = Model.Database.Name;

}
else
{

    server = connectionInfo.Server + ":" + connectionInfo.Port;
    database = Model.Database.ID;

}

Process.Start("C:\\Program Files\\DAX Studio\\DaxStudio.exe", "-s \"" + server + "\" -d \"" + database + "\"");
