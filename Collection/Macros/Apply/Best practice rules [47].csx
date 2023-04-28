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

ScriptHelper.CustomAction("Macros\\Apply\\Best Practice Rules\\Download best practice rules");
ScriptHelper.CustomAction("Macros\\Apply\\Best Practice Rules\\Add long length column annotations");
ScriptHelper.CustomAction("Macros\\Apply\\Best Practice Rules\\Add split datetime annotations");
ScriptHelper.CustomAction("Macros\\Apply\\Best Practice Rules\\Add VertiPaq annotations");

ScriptHelper.Info("Script finished.");