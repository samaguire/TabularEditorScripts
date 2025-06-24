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

    c.RemoveAnnotation("PBI_FormatHint");
    c.RemoveAnnotation("UnderlyingDateTimeDataType");

    switch (c.DataType)
    {

        case DataType.Binary:
            c.FormatString = "\"TRUE\";\"TRUE\";\"FALSE\"";
            break;

        case DataType.DateTime:
            c.FormatString = "d/mm/yyyy";
            c.SetAnnotation("UnderlyingDateTimeDataType", "Date");
            break;

        case DataType.Decimal:
            c.FormatString = "#,0.00";
            break;

        case DataType.Double:
            c.DataType = DataType.Decimal;
            c.FormatString = "#,0.00";
            break;

        case DataType.Int64:
            if (
                c.Name.EndsWith("key", StringComparison.OrdinalIgnoreCase) ||
                c.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase)
            )
            {
                c.FormatString = "0";
            }
            else
            {
                c.FormatString = "#,0";
            }
            break;

        default:
            c.FormatString = string.Empty;
            break;

    }

}