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

ScriptHelper.CustomAction("Macros\\Apply\\Latest Power BI Compatibility Level");
ScriptHelper.CustomAction("Macros\\Apply\\Default column data types and formats");
ScriptHelper.CustomAction("Macros\\Apply\\Encoding hints");
ScriptHelper.CustomAction("Macros\\Apply\\Discourage implicit measures");
ScriptHelper.CustomAction("Macros\\Format all DAX");