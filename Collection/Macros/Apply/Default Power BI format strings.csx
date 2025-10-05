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

string GetFormatString(DataType dataType)
{
    return dataType switch
    {
        DataType.Boolean => "\"TRUE\";\"TRUE\";\"FALSE\"",
        DataType.DateTime => "General Date",
        DataType.Int64 => "0",
        _ => (String)string.Empty
    };
}

foreach (var m in Model.AllMeasures)
{
    m.FormatString = GetFormatString(m.DataType);
    m.FormatStringExpression = string.Empty;
    m.RemoveAnnotation("PBI_FormatHint");
}

foreach (var c in Model.AllColumns)
{
    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }
    c.FormatString = GetFormatString(c.DataType);
    c.RemoveAnnotation("PBI_FormatHint");
    c.RemoveAnnotation("UnderlyingDateTimeDataType");
}