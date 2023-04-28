#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.16\ref\net6.0\System.Windows.Forms.dll"
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
using Newtonsoft.Json.Linq;

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

Func<string, string, string> RemovePBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    if (String.IsNullOrEmpty(pbiChangedProperties)) { return null; }
    var jsonArray = JArray.Parse(pbiChangedProperties);
    jsonArray = new JArray(jsonArray.Values<string>().Where(x => x != propertyName));
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var c in Selected.Columns)
{

    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }

    c.SortByColumn = null;

    var pbiChangedProperties = c.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = RemovePBIChangedProperty(pbiChangedProperties, "SortByColumn");
    if (!String.IsNullOrEmpty(pbiChangedProperties))
    {
        c.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties);
    }
    else
    {
        c.RemoveAnnotation("PBI_ChangedProperties");
    }

}

ScriptHelper.Info("Script finished.");