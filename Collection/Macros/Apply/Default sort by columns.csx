#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
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

Func<string, string, string> RemovePBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    if (String.IsNullOrEmpty(pbiChangedProperties)) { return null; }
    var jsonArray = JArray.Parse(pbiChangedProperties);
    jsonArray = new JArray(jsonArray.Values<string>().Where(x => x != propertyName));
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var c in Model.AllColumns)
{

    // if column is on a calculation group table move to next column
    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }

    // if a valid sortby column can be found use that, otherwsie clear the SortBy column
    var tableColumns = c.Table.Columns.Where(tc =>
        tc.Name.ToLower() == (c.Name + " Number").ToLower() ||
        tc.Name.ToLower() == (c.Name + " Sort").ToLower() ||
        tc.Name.ToLower() == (c.Name + " SortBy").ToLower() ||
        tc.Name.ToLower() == (c.Name + " Order").ToLower() ||
        tc.Name.ToLower() == (c.Name + " OrderBy").ToLower() ||
        tc.Name.ToLower() == (c.Name + " Ordinal").ToLower()
        ).OrderBy(tc => tc);
    if (tableColumns.Any())
    {
        c.SortByColumn = tableColumns.First();
        tableColumns.First().IsHidden = true;
    }
    else
    {
        c.SortByColumn = null;
    }

    // clean up old desktop annotations
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