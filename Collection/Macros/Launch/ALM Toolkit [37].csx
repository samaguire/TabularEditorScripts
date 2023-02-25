#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.14\ref\net6.0\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***

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
using System.Diagnostics;

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

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