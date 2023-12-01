#load "..\..\Management\Common Library.csx"
#load "..\..\Management\Custom Classes.csx"
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

var useShortFormat = true;
var insertSpaceAfterFunctionName = false;
var insertLineBreakOnFirstLine = true;

Func<string, string> GetFormattedDax = (string daxInput) =>
{
    var formattedDax = ScriptHelper.FormatDax(daxInput, shortFormat: useShortFormat, skipSpaceAfterFunctionName: !insertSpaceAfterFunctionName);
    return insertLineBreakOnFirstLine ? "\r\n" + formattedDax : formattedDax;
};

foreach (var m in Model.AllMeasures)
{
    if (!String.IsNullOrEmpty(m.Expression)) { m.Expression = GetFormattedDax(m.Expression); } else { m.Delete(); }
}

foreach (var i in Model.AllCalculationItems)
{
    if (!String.IsNullOrEmpty(i.Expression)) { i.Expression = GetFormattedDax(i.Expression); } else { i.Delete(); }
    if (!String.IsNullOrEmpty(i.FormatStringExpression)) { i.FormatStringExpression = GetFormattedDax(i.FormatStringExpression); }
}

foreach (var t in Model.Tables.OfType<CalculatedTable>().Where(x => !x.Name.StartsWith("DateTableTemplate_") && !x.Name.StartsWith("LocalDateTable_")))
{
    if (!String.IsNullOrEmpty(t.Expression)) { t.Expression = GetFormattedDax(t.Expression); } else { t.Delete(); }
}

foreach (var c in Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.StartsWith("DateTableTemplate_") && !x.DaxTableName.StartsWith("LocalDateTable_")))
{
    if (!String.IsNullOrEmpty(c.Expression)) { c.Expression = GetFormattedDax(c.Expression); } else { c.Delete(); }
}