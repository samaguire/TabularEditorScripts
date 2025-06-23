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
    switch (c.DataType)
    {
        case DataType.Binary:
            c.FormatString = "\"TRUE\";\"TRUE\";\"FALSE\"";
            c.RemoveAnnotation("PBI_FormatHint");
            c.RemoveAnnotation("UnderlyingDateTimeDataType");
            break;
        case DataType.DateTime:
            c.FormatString = "d/mm/yyyy";
            c.RemoveAnnotation("PBI_FormatHint");
            c.SetAnnotation("UnderlyingDateTimeDataType", "Date");
            break;
        case DataType.Decimal:
            c.FormatString = "#,0.00";
            c.RemoveAnnotation("PBI_FormatHint");
            c.RemoveAnnotation("UnderlyingDateTimeDataType");
            break;
        case DataType.Double:
            c.DataType = DataType.Decimal;
            c.FormatString = "#,0.00";
            c.RemoveAnnotation("PBI_FormatHint");
            c.RemoveAnnotation("UnderlyingDateTimeDataType");
            break;
        case DataType.Int64:
            c.FormatString = "#,0";
            c.RemoveAnnotation("PBI_FormatHint");
            c.RemoveAnnotation("UnderlyingDateTimeDataType");
            break;
        default:
            c.FormatString = string.Empty;
            c.RemoveAnnotation("PBI_FormatHint");
            c.RemoveAnnotation("UnderlyingDateTimeDataType");
            break;
    }
}