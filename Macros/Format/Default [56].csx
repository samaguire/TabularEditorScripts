﻿#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

Func<string, string, string> RemovePBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    if (String.IsNullOrEmpty(pbiChangedProperties)) { return null; }
    var jsonArray = JArray.Parse(pbiChangedProperties);
    jsonArray = new JArray(jsonArray.Values<string>().Where(x => x != propertyName));
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var c in Selected.Columns)
{

    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }

    switch (c.DataType)
    {
        case DataType.Boolean:
            c.FormatString = "\"TRUE\";\"TRUE\";\"FALSE\"";
            c.SetAnnotation("Format", "<Format Format=\"Boolean\" />");
            break;
        case DataType.DateTime:
            c.FormatString = "General Date";
            // c.SetAnnotation("Format", "<Format Format=\"DateTimeGeneralPattern\"><DateTimes><DateTime LCID=\"5129\" Group=\"GeneralDateTimeLong\" FormatString=\"G\" /></DateTimes></Format>");
            c.SetAnnotation("Format", "<Format Format=\"DateTimeGeneralPattern\"><DateTimes><DateTime LCID=\"1033\" Group=\"GeneralDateTimeLong\" FormatString=\"G\" /></DateTimes></Format>");
            c.RemoveAnnotation("UnderlyingDateTimeDataType");
            break;
        case DataType.Decimal:
            c.FormatString = "\\$#,0.###############;-\\$#,0.###############;\\$#,0.###############";
            // c.SetAnnotation("Format", "<Format Format=\"CurrencyGeneral\" ThousandSeparator=\"True\"><Currency LCID=\"5129\" DisplayName=\"$ English (New Zealand)\" Symbol=\"$\" PositivePattern=\"0\" NegativePattern=\"1\" /></Format>");
            // c.SetAnnotation("PBI_FormatHint", "{\"currencyCulture\":\"en-NZ\"}");
            c.SetAnnotation("Format", "<Format Format=\"CurrencyGeneral\" ThousandSeparator=\"True\"><Currency LCID=\"1033\" DisplayName=\"Currency General\" Symbol=\"$\" PositivePattern=\"0\" NegativePattern=\"0\" /></Format>");
            c.RemoveAnnotation("PBI_FormatHint");
            break;
        case DataType.Double:
            c.FormatString = "";
            c.SetAnnotation("Format", "<Format Format=\"General\" />");
            break;
        case DataType.Int64:
            c.FormatString = "0";
            c.SetAnnotation("Format", "<Format Format=\"NumberWhole\" Accuracy=\"0\" />");
            break;
        case DataType.String:
            c.FormatString = "";
            c.SetAnnotation("Format", "<Format Format=\"Text\" />");
            break;
        default:
            break;
    }

    var pbiChangedProperties = c.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = RemovePBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties))
    {
        c.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties);
    }
    else
    {
        c.RemoveAnnotation("PBI_ChangedProperties");
    }

    c.RemoveAnnotation("disallowApplyingDefaultFormatting");

}

ScriptHelper.Info("Script finished.");
