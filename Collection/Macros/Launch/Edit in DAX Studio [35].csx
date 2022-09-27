#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.9\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/

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

var connectionInfo = ScriptHost.Model.Database.TOMDatabase.Server.ConnectionInfo;
string server = null;
string database = null;

if (connectionInfo.Port == null)
{
    server = connectionInfo.Server;
    database = ScriptHost.Model.Database.Name;
}
else
{
    server = connectionInfo.Server + ":" + connectionInfo.Port;
    database = ScriptHost.Model.Database.ID;
}

// Launch DAX Studio with the -f argument to load the file, and the -s and -d arguments to connect to Analysis Services:

Process.Start("C:\\Program Files\\DAX Studio\\DaxStudio.exe", "-s \"" + server + "\" -d \"" + database + "\" -f \"" + file + "\"");
