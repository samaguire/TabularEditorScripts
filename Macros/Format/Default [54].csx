#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
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

// foreach (var m in Model.AllMeasures)
foreach (var m in Selected.Measures)
{

    // if (!fullReset)
    // {
    //     bool disallowApplyingDefaultFormatting = Convert.ToBoolean(m.GetAnnotation("disallowApplyingDefaultFormatting"));
    //     if (disallowApplyingDefaultFormatting) { continue; }
    // }

    m.FormatString = "";
    m.SetAnnotation("Format", "<Format Format=\"General\" />");
    m.SetAnnotation("PBI_FormatHint", "{\"isGeneralNumber\":true}");

    string textPBI_ChangedProperties = m.GetAnnotation("PBI_ChangedProperties");
    if (!String.IsNullOrEmpty(textPBI_ChangedProperties))
    {
        textPBI_ChangedProperties = RemovePBIChangedProperty(textPBI_ChangedProperties, "FormatString");
        if (textPBI_ChangedProperties=="[\"\"]")
        {
            m.RemoveAnnotation("PBI_ChangedProperties");
        }
        else
        {
            m.SetAnnotation("PBI_ChangedProperties", textPBI_ChangedProperties);
        }
    }

    // if (!fullReset)
    // {
        m.RemoveAnnotation("disallowApplyingDefaultFormatting");
    // }

}

ScriptHelper.Info("Script finished.");
