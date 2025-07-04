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

foreach (var c in Model.AllColumns.Where(x => x.SummarizeBy != AggregateFunction.None))
{
    c.SummarizeBy = AggregateFunction.None;
}

Model.DiscourageImplicitMeasures = true;