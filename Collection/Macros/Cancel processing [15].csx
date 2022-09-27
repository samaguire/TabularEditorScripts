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

using System.Data;

string databaseId = ScriptHost.Model.Database.ID;
string databaseName = ScriptHost.Model.Database.Name;
DataTable dtDmvCmd = ScriptHost.ExecuteDax("SELECT [SESSION_ID],[SESSION_LAST_COMMAND] FROM $SYSTEM.DISCOVER_SESSIONS").Tables[0];
string sId = null;

foreach (DataRow drDmvCmd in dtDmvCmd.Rows)
{
    var sessionId = drDmvCmd["SESSION_ID"].ToString();
    var sessionCmd = drDmvCmd["SESSION_LAST_COMMAND"].ToString();
    if (sessionCmd.StartsWith("<Batch Transaction=") && sessionCmd.Contains("<Refresh xmlns") && sessionCmd.Contains("<DatabaseID>" + databaseId + "</DatabaseID>")) { sId = sessionId; }
}

if (sId == null)
{
    ScriptHost.Error("No processing Session ID found for the '" + databaseName + "' ScriptHost.Model.");
}
else
{
    ScriptHost.Model.Database.TOMDatabase.Server.CancelSession(sId);
    ScriptHost.Info("Processing for the '" + databaseName + "' model has been cancelled (Session ID: " + sId + ").");
}
