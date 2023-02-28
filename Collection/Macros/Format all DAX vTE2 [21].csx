﻿#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.14\ref\net6.0\System.Windows.Forms.dll"
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

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

var useShortFormat = false;
var insertSpaceAfterFunctionName = false;
var insertLineBreakOnFirstLine = true;

foreach (var m in Model.AllMeasures) { ScriptHelper.FormatDax(m); }
foreach (var i in Model.AllCalculationItems) { ScriptHelper.FormatDax(i); }
foreach (var t in Model.Tables.OfType<CalculatedTable>().Where(x => !x.Name.Contains("DateTableTemplate_") && !x.Name.Contains("LocalDateTable_"))) { ScriptHelper.FormatDax(t); }
foreach (var c in Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.Contains("DateTableTemplate_") && !x.DaxTableName.Contains("LocalDateTable_"))) { ScriptHelper.FormatDax(c); }

ScriptHelper.CallDaxFormatter(shortFormat: useShortFormat, skipSpaceAfterFunctionName: !insertSpaceAfterFunctionName);

if (insertLineBreakOnFirstLine)
{
    foreach (var m in Model.AllMeasures) { m.Expression = "\r\n" + m.Expression; }
    foreach (var i in Model.AllCalculationItems) { i.Expression = "\r\n" + i.Expression; }
    foreach (var t in Model.Tables.OfType<CalculatedTable>().Where(x => !x.Name.Contains("DateTableTemplate_") && !x.Name.Contains("LocalDateTable_"))) { t.Expression = "\r\n" + t.Expression; }
    foreach (var c in Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.Contains("DateTableTemplate_") && !x.DaxTableName.Contains("LocalDateTable_"))) { c.Expression = "\r\n" + c.Expression; }
}

ScriptHelper.Info("Script finished.");