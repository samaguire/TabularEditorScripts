#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
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
using Newtonsoft.Json.Linq;

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

Func<string, string, string> AddPBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    var jsonArray = new JArray();
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { jsonArray = JArray.Parse(pbiChangedProperties); }
    if (!String.IsNullOrEmpty(propertyName)) { jsonArray.Add(new JValue(propertyName)); }
    jsonArray = new JArray(jsonArray.Distinct());
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var m in Selected.Measures)
{

    if (m.DataType != DataType.Decimal && m.DataType != DataType.Double && m.DataType != DataType.Int64) { continue; }

    m.FormatString = "#,0";
    m.SetAnnotation("Format", "<Format Format=\"NumberWhole\" Accuracy=\"0\" ThousandSeparator=\"True\" />");
    m.RemoveAnnotation("PBI_FormatHint");

    var pbiChangedProperties = m.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = AddPBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { m.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties); }

    m.SetAnnotation("DisallowApplyingDefaultFormatting", "true");

}

foreach (var c in Selected.Columns)
{

    if (c.DataType != DataType.Decimal && c.DataType != DataType.Double && c.DataType != DataType.Int64) { continue; }

    c.FormatString = "#,0";
    c.SetAnnotation("Format", "<Format Format=\"NumberWhole\" Accuracy=\"0\" ThousandSeparator=\"True\" />");

    var pbiChangedProperties = c.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = AddPBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { c.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties); }

    c.SetAnnotation("DisallowApplyingDefaultFormatting", "true");

}

ScriptHelper.Info("Script finished.");