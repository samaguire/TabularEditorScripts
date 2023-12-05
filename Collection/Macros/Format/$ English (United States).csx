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

var formatString = "\\$#,0.00;(\\$#,0.00);\\$#,0.00";

foreach (var m in Selected.Measures)
{
    if (m.DataType != DataType.Decimal && m.DataType != DataType.Double && m.DataType != DataType.Int64 && m.DataType != DataType.Variant) { continue; }
    m.FormatString = formatString;
}

foreach (var c in Selected.Columns)
{
    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }
    if (c.DataType != DataType.Decimal && c.DataType != DataType.Double && c.DataType != DataType.Int64) { continue; }
    c.FormatString = formatString;
}