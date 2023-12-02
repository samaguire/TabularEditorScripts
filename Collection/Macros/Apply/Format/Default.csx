#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
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

Func<string, string, string> RemovePBIChangedProperty = (string pbiChangedProperties, string propertyName) =>
{
    if (String.IsNullOrEmpty(pbiChangedProperties)) { return null; }
    var jsonArray = JArray.Parse(pbiChangedProperties);
    jsonArray = new JArray(jsonArray.Values<string>().Where(x => x != propertyName));
    return jsonArray.Any() ? JsonConvert.SerializeObject(jsonArray) : null;
};

foreach (var m in Selected.Measures)
{

    m.FormatString = string.Empty;
    m.FormatStringExpression = string.Empty;
    m.SetAnnotation("Format", "<Format Format=\"General\" />");
    m.SetAnnotation("PBI_FormatHint", "{\"isGeneralNumber\":true}");

    var pbiChangedProperties = m.GetAnnotation("PBI_ChangedProperties");
    pbiChangedProperties = RemovePBIChangedProperty(pbiChangedProperties, "FormatString");
    if (!String.IsNullOrEmpty(pbiChangedProperties))
    {
        m.SetAnnotation("PBI_ChangedProperties", pbiChangedProperties);
    }
    else
    {
        m.RemoveAnnotation("PBI_ChangedProperties");
    }

    m.RemoveAnnotation("DisallowApplyingDefaultFormatting");

}

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
            c.FormatString = "Long Date";
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
            c.FormatString = string.Empty;
            c.SetAnnotation("Format", "<Format Format=\"General\" />");
            break;
        case DataType.Int64:
            c.FormatString = "0";
            c.SetAnnotation("Format", "<Format Format=\"NumberWhole\" Accuracy=\"0\" />");
            break;
        case DataType.String:
            c.FormatString = string.Empty;
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

    c.RemoveAnnotation("DisallowApplyingDefaultFormatting");

}