#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "Microsoft.AnalysisServices.Core.dll"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using System.Diagnostics;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

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

Process.Start("C:\\Program Files (x86)\\Tabular Editor\\TabularEditor.exe", "\"" + server + "\" \"" + database + "\"");
