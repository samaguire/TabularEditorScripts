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

foreach (var c in Selected.Columns)
{

    if (c.Table.ObjectType == ObjectType.CalculationGroupTable) { continue; }
    if (c.DataType != DataType.Decimal && c.DataType != DataType.Double && c.DataType != DataType.Int64) { continue; }

    c.FormatString = "\\$#,0.00;-\\$#,0.00;\\$#,0.00";
    c.SetAnnotation("Format", "<Format Format=\"CurrencyCustom\" Accuracy=\"2\" ThousandSeparator=\"True\"><Currency LCID=\"5129\" DisplayName=\"$ English (New Zealand)\" Symbol=\"$\" PositivePattern=\"0\" NegativePattern=\"1\" /></Format>");

    string textPBI_ChangedProperties = c.GetAnnotation("PBI_ChangedProperties") ?? "";
    textPBI_ChangedProperties = AddToPBI_ChangedProperties(textPBI_ChangedProperties, "FormatString");
    c.SetAnnotation("PBI_ChangedProperties", textPBI_ChangedProperties);

    c.SetAnnotation("disallowApplyingDefaultFormatting", "true");

}

ScriptHelper.Info("Script finished.");
