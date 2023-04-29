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
using Newtonsoft.Json.Linq;

Func<string, string, string> RemovePBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    if (String.IsNullOrEmpty(pbiChangedProperties)) { return null; }
    var jsonArray = JArray.Parse(pbiChangedProperties);
    jsonArray = new JArray(jsonArray.Values<string>().Where(x => x != propertyName));
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var c in Model.AllColumns.Where(x => x.SortByColumn != null))
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