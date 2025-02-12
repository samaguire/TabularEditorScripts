#load "..\..\..\Management\Common Library.csx"
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

foreach (var c in Model.AllColumns)
{

    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }

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

}