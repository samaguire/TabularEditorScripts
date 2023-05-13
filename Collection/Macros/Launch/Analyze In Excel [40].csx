#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Common Classes.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.VisualBasic"

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
using Microsoft.VisualBasic;
using System.Diagnostics;

// https://github.com/TabularEditor/TabularEditor3/issues/249#issuecomment-939848828
// Works with version 1.1.3 when MSOLAP isn't separately installed as per Analyze In Excel warning.

var connectionInfo = Model.Database.TOMDatabase.Server.ConnectionInfo;
string server = null;
string database = null;

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