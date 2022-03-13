﻿#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

Func<string, string, string> AddToPBI_ChangedProperties = (string textPBI_ChangedProperties, string textProperty) =>
{
    List<string> textProperties = textPBI_ChangedProperties.Trim('[').Trim(']').Replace("\"", "").Split(',').ToList();
    textProperties = textProperties.Where(p => p != "").ToList();
    if (!textProperties.Contains(textProperty)) { textProperties.Add(textProperty); }
    return "[\"" + String.Join("\",\"", textProperties) + "\"]";
};

foreach (var c in Selected.Columns)
{

    Column sortByColumn = ScriptHelper.SelectColumn(c.Table, null, "Select sort by column for " + c.DaxObjectFullName + ":");
    if (sortByColumn == null) { continue; }

    if (sortByColumn != c) { c.SortByColumn = sortByColumn; }

    string textPBI_ChangedProperties = c.GetAnnotation("PBI_ChangedProperties") ?? "";
    textPBI_ChangedProperties = AddToPBI_ChangedProperties(textPBI_ChangedProperties, "SortByColumn");
    c.SetAnnotation("PBI_ChangedProperties", textPBI_ChangedProperties);

}

ScriptHelper.Info("Script finished.");
