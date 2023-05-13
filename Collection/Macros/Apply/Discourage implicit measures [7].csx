#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Common Classes.csx"
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

foreach (var c in Model.AllColumns.Where(x => x.SummarizeBy != AggregateFunction.None))
{
    c.SummarizeBy = AggregateFunction.None;
    c.SetAnnotation("SummarizationSetBy", "User");
}

Model.DiscourageImplicitMeasures = true;

ScriptHelper.Info("Script finished.");