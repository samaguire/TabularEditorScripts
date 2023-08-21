#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
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

foreach (var m in Model.AllMeasures) { m.LineageTag = null; }
foreach (var l in Model.AllLevels) { l.LineageTag = null; }
foreach (var h in Model.AllHierarchies) { h.LineageTag = null; }
foreach (var c in Model.AllColumns) { c.LineageTag = null; }
foreach (var t in Model.Tables) { t.LineageTag = null; }

ScriptHelper.Info("Script finished.");