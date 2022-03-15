#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

Func<string, string, string> AddToPBI_ChangedProperties = (string textPBI_ChangedProperties, string textProperty) =>
{
    List<string> textProperties = textPBI_ChangedProperties.Trim('[', ']').Replace("\"", "").Split(',').ToList();
    textProperties = textProperties.Where(p => p != "").ToList();
    if (!textProperties.Contains(textProperty)) { textProperties.Add(textProperty); }
    return "[\"" + String.Join("\",\"", textProperties) + "\"]";
};

foreach (var m in Selected.Measures)
{

    if (m.DataType != DataType.Decimal && m.DataType != DataType.Double && m.DataType != DataType.Int64) { continue; }

    m.FormatString = "\\$#,0.00;(\\$#,0.00);\\$#,0.00";
    m.SetAnnotation("Format", "<Format Format=\"CurrencyCustom\" Accuracy=\"2\" ThousandSeparator=\"True\"><Currency LCID=\"1033\" DisplayName=\"$ English (United States)\" Symbol=\"$\" PositivePattern=\"0\" NegativePattern=\"0\" /></Format>");
    m.RemoveAnnotation("PBI_FormatHint");

    string textPBI_ChangedProperties = m.GetAnnotation("PBI_ChangedProperties") ?? "";
    textPBI_ChangedProperties = AddToPBI_ChangedProperties(textPBI_ChangedProperties, "FormatString");
    m.SetAnnotation("PBI_ChangedProperties", textPBI_ChangedProperties);

    m.SetAnnotation("disallowApplyingDefaultFormatting", "true");

}

ScriptHelper.Info("Script finished.");
