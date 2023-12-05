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

string GetFormatString(DataType dataType)
{
    switch (dataType)
    {
        case DataType.Boolean:
            return "\"TRUE\";\"TRUE\";\"FALSE\"";
        case DataType.DateTime:
            return "General Date";
        case DataType.Decimal:
            return "\\$#,0.###############;-\\$#,0.###############;\\$#,0.###############";
            // return "$ #,##0.00"; // TE3 version
        case DataType.Double:
            return string.Empty;
        case DataType.Int64:
            return "0";
        case DataType.String:
            return string.Empty;
        case DataType.Variant:
            return string.Empty;
        default:
            return string.Empty;
    }
}

foreach (var m in Model.AllMeasures)
{
    m.FormatString = GetFormatString(m.DataType);
    m.FormatStringExpression = string.Empty;
}

foreach (var c in Model.AllColumns)
{
    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }
    c.FormatString = GetFormatString(c.DataType);
}