#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "Microsoft.AnalysisServices.Core.dll"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***
using TabularEditor.Shared;
using System.IO;
using System.Diagnostics;
using System.Reflection;

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

// https://github.com/TabularEditor/TabularEditor3/issues/249#issuecomment-939848828



// Get the current document:

var document = ((dynamic)SharedApp.Instance).ViewService.CurrentDocument;
if (document == null) { return; }

// Get the code within the current document:

var code = null as string;
try { code = document.Content.ToString(); } catch { return; }

// Save a (temporary) file:

var file = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), document.Caption.ToString()), ".dax");
File.WriteAllText(file, code);

// Get Analysis Services connection details:

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

// Launch DAX Studio with the -f argument to load the file, and the -s and -d arguments to connect to Analysis Services:

Process.Start("C:\\Program Files\\DAX Studio\\DaxStudio.exe", "-s \"" + server + "\" -d \"" + database + "\" -f \"" + file + "\"");
