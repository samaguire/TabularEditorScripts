#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

Func<string, string, string> AddToPBI_ChangedProperties = (string textPBI_ChangedProperties, string textProperty) =>
{
    List<string> textProperties = textPBI_ChangedProperties.Trim('[', ']').Replace("\"", "").Split(',').ToList();
    textProperties = textProperties.Where(p => p != "").ToList();
    if (!textProperties.Contains(textProperty)) { textProperties.Add(textProperty); }
    return "[\"" + String.Join("\",\"", textProperties) + "\"]";
};

Func<string, string, string> RemoveFromPBI_ChangedProperties = (string textPBI_ChangedProperties, string textProperty) =>
{
    //var jsonArray = JArray.Parse("[\"FormatString\"]");
    var jsonArray = JArray.Parse("");
    
    ScriptHelper.Output(jsonArray);
    var jsonArray2 = jsonArray.Values<string>().Where(x => x != "FormatString");
    ScriptHelper.Output(jsonArray2);
    return JsonConvert.SerializeObject(jsonArray2);
};

foreach (var m in Selected.Measures)
{
    string textPBI_ChangedProperties = m.GetAnnotation("PBI_ChangedProperties");
    RemoveFromPBI_ChangedProperties(textPBI_ChangedProperties, "FormatString").Output();
    // string textPBI_ChangedProperties = m.GetAnnotation("PBI_ChangedProperties") ?? "";
    // AddToPBI_ChangedProperties(textPBI_ChangedProperties, "FormatString").Output();
}