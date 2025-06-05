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

var formatString = "Long Date";

foreach (var m in Selected.Measures)
{
    if (m.DataType != DataType.DateTime && m.DataType != DataType.Variant) { continue; }
    m.FormatString = formatString;
}

foreach (var c in Selected.Columns)
{
    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }
    if (c.DataType != DataType.DateTime) { continue; }
    c.FormatString = formatString;
}