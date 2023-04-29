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

Func<string, string, string> AddPBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    var jsonArray = new JArray();
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { jsonArray = JArray.Parse(pbiChangedProperties); }
    if (!String.IsNullOrEmpty(propertyName)) { jsonArray.Add(new JValue(propertyName)); }
    jsonArray = new JArray(jsonArray.Distinct());
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var c in Selected.Columns)
{

    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }

    var tableColumns = c.Table.Columns.Where(tc => tc != c);
    Column sortByColumn = ScriptHelper.SelectColumn(tableColumns, null, "Select sort by column for " + c.DaxObjectFullName + ":");
    if (sortByColumn == null) { continue; }
    if (sortByColumn != c) { c.SortByColumn = sortByColumn; }

    var pbiChangedProperties = c.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = AddPBIChangedProperty(pbiChangedProperties, "SortByColumn");
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { c.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties); }

}

ScriptHelper.Info("Script finished.");