#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.9\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
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

Func<string, string> GetFormattedDax = (string daxInput) =>
{
    var formattedDax = ScriptHost.FormatDax(daxInput, shortFormat: useShortFormat, skipSpaceAfterFunctionName: !insertSpaceAfterFunctionName);
    return insertLineBreakOnFirstLine ? "\r\n" + formattedDax : formattedDax;
};

foreach (var m in ScriptHost.Model.AllMeasures)
{
    if (!String.IsNullOrEmpty(m.Expression)) { m.Expression = GetFormattedDax(m.Expression); } else { m.Delete(); }
}

foreach (var i in ScriptHost.Model.AllCalculationItems)
{
    if (!String.IsNullOrEmpty(i.Expression)) { i.Expression = GetFormattedDax(i.Expression); } else { i.Delete(); }
    if (!String.IsNullOrEmpty(i.FormatStringExpression)) { i.FormatStringExpression = GetFormattedDax(i.FormatStringExpression); }
}

foreach (var t in ScriptHost.Model.Tables.OfType<CalculatedTable>().Where(x => !x.Name.StartsWith("DateTableTemplate_") && !x.Name.StartsWith("LocalDateTable_")))
{
    if (!String.IsNullOrEmpty(t.Expression)) { t.Expression = GetFormattedDax(t.Expression); } else { t.Delete(); }
}

foreach (var c in ScriptHost.Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.StartsWith("DateTableTemplate_") && !x.DaxTableName.StartsWith("LocalDateTable_")))
{
    if (!String.IsNullOrEmpty(c.Expression)) { c.Expression = GetFormattedDax(c.Expression); } else { c.Delete(); }
}

ScriptHost.Info("Script finished.");
