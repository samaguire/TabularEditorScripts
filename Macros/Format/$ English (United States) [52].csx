#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

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

    m.FormatString = "\\$#,0.00;(\\$#,0.00);\\$#,0.00";
    m.SetAnnotation("Format", "<Format Format=\"CurrencyCustom\" Accuracy=\"2\" ThousandSeparator=\"True\"><Currency LCID=\"1033\" DisplayName=\"$ English (United States)\" Symbol=\"$\" PositivePattern=\"0\" NegativePattern=\"0\" /></Format>");
    m.RemoveAnnotation("PBI_FormatHint");

    var pbiChangedProperties = m.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = AddPBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties)) { m.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties); }

    m.SetAnnotation("DisallowApplyingDefaultFormatting", "true");

}

ScriptHelper.Info("Script finished.");
