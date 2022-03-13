#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

Func<string, string, string> RemoveFromPBI_ChangedProperties = (string textPBI_ChangedProperties, string textProperty) =>
{
    List<string> textProperties = textPBI_ChangedProperties.Trim('[').Trim(']').Replace("\"", "").Split(',').ToList();
    textProperties = textProperties.Where(p => p != textProperty).ToList();
    return "[\"" + String.Join("\",\"", textProperties) + "\"]";
};

foreach (var c in Selected.Columns)
{

    c.SortByColumn = null;

    string textPBI_ChangedProperties = c.GetAnnotation("PBI_ChangedProperties");
    if (!String.IsNullOrEmpty(textPBI_ChangedProperties))
    {
        textPBI_ChangedProperties = RemoveFromPBI_ChangedProperties(textPBI_ChangedProperties, "SortByColumn");
        if (textPBI_ChangedProperties=="[\"\"]")
        {
            c.RemoveAnnotation("PBI_ChangedProperties");
        }
        else
        {
            c.SetAnnotation("PBI_ChangedProperties", textPBI_ChangedProperties);
        }
    }

}

ScriptHelper.Info("Script finished.");
