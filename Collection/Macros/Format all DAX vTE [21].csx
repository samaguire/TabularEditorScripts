#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.11\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/

var useShortFormat = true;
var insertSpaceAfterFunctionName = false;
var insertLineBreakOnFirstLine = true;

foreach (var m in ScriptHost.Model.AllMeasures) { ScriptHost.FormatDax(m); }
foreach (var i in ScriptHost.Model.AllCalculationItems) { ScriptHost.FormatDax(i); }
foreach (var t in ScriptHost.Model.Tables.OfType<CalculatedTable>().Where(x => !x.Name.Contains("DateTableTemplate_") && !x.Name.Contains("LocalDateTable_"))) { ScriptHost.FormatDax(t); }
foreach (var c in ScriptHost.Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.Contains("DateTableTemplate_") && !x.DaxTableName.Contains("LocalDateTable_"))) { ScriptHost.FormatDax(c); }

ScriptHost.CallDaxFormatter(shortFormat: useShortFormat, skipSpaceAfterFunctionName: !insertSpaceAfterFunctionName);

if (insertLineBreakOnFirstLine)
{
    foreach (var m in ScriptHost.Model.AllMeasures) { m.Expression = "\r\n" + m.Expression; }
    foreach (var i in ScriptHost.Model.AllCalculationItems) { i.Expression = "\r\n" + i.Expression; }
    foreach (var t in ScriptHost.Model.Tables.OfType<CalculatedTable>().Where(x => !x.Name.Contains("DateTableTemplate_") && !x.Name.Contains("LocalDateTable_"))) { t.Expression = "\r\n" + t.Expression; }
    foreach (var c in ScriptHost.Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.Contains("DateTableTemplate_") && !x.DaxTableName.Contains("LocalDateTable_"))) { c.Expression = "\r\n" + c.Expression; }
}

ScriptHost.Info("Script finished.");