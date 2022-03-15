#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "Microsoft.AnalysisServices.Core.dll"
#r "System.Xml"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using Microsoft.AnalysisServices.Core;
using System.Xml;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// https://www.elegantbi.com/post/canceldatarefreshte



var DMV_Cmd = ScriptHelper.ExecuteDax("SELECT [SESSION_ID],[SESSION_LAST_COMMAND] FROM $SYSTEM.DISCOVER_SESSIONS").Tables[0];
bool runTMSL = true;
string databaseID = Model.Database.ID;
string databaseName = Model.Database.Name;
string sID = string.Empty;

for (int r = 0; r < DMV_Cmd.Rows.Count; r++)
{
    string sessionID = DMV_Cmd.Rows[r][0].ToString();
    string cmdText = DMV_Cmd.Rows[r][1].ToString();
    
    // Capture refresh command for the database
    if (cmdText.StartsWith("<Batch Transaction=") && cmdText.Contains("<Refresh xmlns") && cmdText.Contains("<DatabaseID>"+databaseID+"</DatabaseID>"))
    {
        sID = sessionID;
    }      
}

if (sID == string.Empty)
{
    ScriptHelper.Error("No processing Session ID found for the '"+databaseName+"' model.");
    return;
}

if (runTMSL)
{
    Model.Database.TOMDatabase.Server.CancelSession(sID);
    ScriptHelper.Info("Processing for the '"+databaseName+"' model has been cancelled (Session ID: "+sID+").");
}
else
{
    sID.Output();
}
