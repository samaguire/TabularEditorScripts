#load "..\..\..\Management\Common Library.csx"
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

Process.Start("C:\\Program Files\\Power BI ALM Toolkit\\Power BI ALM Toolkit\\AlmToolkit.exe", "\"" + server + "\" \"" + database + "\"");