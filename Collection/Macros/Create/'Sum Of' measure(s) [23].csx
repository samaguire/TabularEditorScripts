﻿#load "..\..\..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.VisualBasic"

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
using Microsoft.VisualBasic;

foreach(var c in Selected.Columns)
{
    c.Table.AddMeasure(
        name: "Sum Of " + c.Name,
        expression: "SUM( " + c.DaxObjectFullName + " )",
        displayFolder: c.DisplayFolder + "\\Sum Of Measures"
    );
    c.IsHidden = true;
}

ScriptHelper.Info("Script finished.");