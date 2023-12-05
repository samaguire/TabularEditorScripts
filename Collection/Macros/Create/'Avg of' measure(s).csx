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

foreach(var c in Selected.Columns)
{
    c.Table.AddMeasure(
        name: $"Avg of {c.Name}",
        expression: $"AVERAGE( {c.DaxObjectFullName} )",
        displayFolder: string.IsNullOrEmpty(c.DisplayFolder) ? $"Avg of Measures" : $"{c.DisplayFolder}\\Avg of Measures"
    );
    c.IsHidden = true;
}