#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

foreach (var m in Selected.Measures)
{
    if (String.IsNullOrEmpty(m.GetAnnotation("DisallowApplyingDefaultFormatting")))
    {
        m.SetAnnotation("DisallowApplyingDefaultFormatting", "true");
    }
    else
    {
        m.RemoveAnnotation("DisallowApplyingDefaultFormatting");
    }
}

foreach (var c in Selected.Columns)
{
    if (String.IsNullOrEmpty(c.GetAnnotation("DisallowApplyingDefaultFormatting")))
    {
        c.SetAnnotation("DisallowApplyingDefaultFormatting", "true");
    }
    else
    {
        c.RemoveAnnotation("DisallowApplyingDefaultFormatting");
    }
}

ScriptHelper.Info("Script finished.");
