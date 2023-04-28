#load "..\..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.AnalysisServices.Core"

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