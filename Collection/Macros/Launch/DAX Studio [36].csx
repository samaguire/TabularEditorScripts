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

Process.Start("C:\\Program Files\\DAX Studio\\DaxStudio.exe", "-s \"" + server + "\" -d \"" + database + "\"");
