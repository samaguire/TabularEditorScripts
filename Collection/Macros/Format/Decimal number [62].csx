#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.12\ref\net6.0\System.Windows.Forms.dll"
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

using Newtonsoft.Json.Linq;

Func<string, string, string> AddPBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    var jsonArray = new JArray();
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { jsonArray = JArray.Parse(pbiChangedProperties); }
    if (!String.IsNullOrEmpty(propertyName)) { jsonArray.Add(new JValue(propertyName)); }
    jsonArray = new JArray(jsonArray.Distinct());
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var m in ScriptHost.Selected.Measures)
{

    if (m.DataType != DataType.Decimal && m.DataType != DataType.Double && m.DataType != DataType.Int64) { continue; }

    m.FormatString = "#,0.00";
    m.SetAnnotation("Format", "<Format Format=\"NumberDecimal\" Accuracy=\"2\" ThousandSeparator=\"True\" />");
    m.RemoveAnnotation("PBI_FormatHint");

    var pbiChangedProperties = m.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = AddPBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { m.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties); }

    m.SetAnnotation("DisallowApplyingDefaultFormatting", "true");

}

foreach (var c in ScriptHost.Selected.Columns)
{

    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }
    if (c.DataType != DataType.Decimal && c.DataType != DataType.Double && c.DataType != DataType.Int64) { continue; }

    c.FormatString = "#,0.00";
    c.SetAnnotation("Format", "<Format Format=\"NumberDecimal\" Accuracy=\"2\" ThousandSeparator=\"True\" />");

    var pbiChangedProperties = c.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = AddPBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { c.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties); }

    c.SetAnnotation("DisallowApplyingDefaultFormatting", "true");

}

ScriptHost.Info("Script finished.");