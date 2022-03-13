#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "Microsoft.AnalysisServices.Core.dll"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using System.Diagnostics;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// https://github.com/TabularEditor/TabularEditor3/issues/249#issuecomment-939848828

// Works with version 1.1.3 when MSOLAP isn't separately installed as per Analyze In Excel warning.



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

Process.Start("C:\\Program Files (x86)\\Sqlbi\\Analyze in Excel for Power BI Desktop\\AnalyzeInExcel.exe", "--server=\"" + server + "\" --database=\"" + database + "\" --telemetry");
