#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Windows.Forms\v4.0_4.0.0.0__b77a5c561934e089\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.AnalysisServices.Core.dll"

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.UI;
using TabularEditor.Scripting;
// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***
using Microsoft.AnalysisServices.Core;
using System.Data;

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

string databaseId = Model.Database.ID;
string databaseName = Model.Database.Name;
DataTable dtDmvCmd = ScriptHelper.ExecuteDax("SELECT [SESSION_ID],[SESSION_LAST_COMMAND] FROM $SYSTEM.DISCOVER_SESSIONS").Tables[0];
string sId = null;

foreach (DataRow drDmvCmd in dtDmvCmd.Rows)
{
    var sessionId = drDmvCmd["SESSION_ID"].ToString();
    var sessionCmd = drDmvCmd["SESSION_LAST_COMMAND"].ToString();
    if (sessionCmd.StartsWith("<Batch Transaction=") && sessionCmd.Contains("<Refresh xmlns") && sessionCmd.Contains("<DatabaseID>" + databaseId + "</DatabaseID>")) { sId = sessionId; }
}

if (sId == null)
{
    ScriptHelper.Error("No processing Session ID found for the '" + databaseName + "' Model.");
}
else
{
    Model.Database.TOMDatabase.Server.CancelSession(sId);
    ScriptHelper.Info("Processing for the '" + databaseName + "' model has been cancelled (Session ID: " + sId + ").");
}