﻿#load "..\..\..\Management\Common Library.csx"
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
    //c.Table.AddMeasure(
    //    name: $"Count of {c.Name}",
    //    expression: $"DISTINCTCOUNT( {c.DaxObjectFullName} )",
    //    displayFolder: string.IsNullOrEmpty(c.DisplayFolder) ? $"Count of Measures" : $"{c.DisplayFolder}\\Count of Measures"
    //);
    c.Table.AddMeasure(
        name: $"{c.Name} Count",
        expression: $"DISTINCTCOUNT( {c.DaxObjectFullName} )",
        displayFolder: string.IsNullOrEmpty(c.DisplayFolder) ? $"› Measures" : $"{c.DisplayFolder}\\› Measures"
    );
    // c.IsHidden = true;
}