#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

var useShortFormat = true;
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
