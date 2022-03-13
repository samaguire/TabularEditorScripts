#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

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
    if (c.DataType != DataType.DateTime) { continue; }

    c.FormatString = "Short Time";
    c.SetAnnotation("Format", "<Format Format=\"DateTimeGeneralPattern\"><DateTimes><DateTime LCID=\"5129\" Group=\"ShortTime\" FormatString=\"t\" /></DateTimes></Format>");
    c.SetAnnotation("UnderlyingDateTimeDataType", "Time");

    string textPBI_ChangedProperties = c.GetAnnotation("PBI_ChangedProperties") ?? "";
    textPBI_ChangedProperties = AddToPBI_ChangedProperties(textPBI_ChangedProperties, "FormatString");
    c.SetAnnotation("PBI_ChangedProperties", textPBI_ChangedProperties);

    c.SetAnnotation("disallowApplyingDefaultFormatting", "true");

}

ScriptHelper.Info("Script finished.");
