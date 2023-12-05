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

// not a complete list
foreach (var m in Model.AllMeasures) { m.LineageTag = String.Empty; }
foreach (var l in Model.AllLevels) { l.LineageTag = String.Empty; }
foreach (var h in Model.AllHierarchies) { h.LineageTag = String.Empty; }
foreach (var c in Model.AllColumns) { c.LineageTag = String.Empty; }
foreach (var t in Model.Tables) { t.LineageTag = String.Empty; }