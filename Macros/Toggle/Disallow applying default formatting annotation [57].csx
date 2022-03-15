#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

foreach (var m in Selected.Measures)
{
    if (String.IsNullOrEmpty(m.GetAnnotation("disallowApplyingDefaultFormatting")))
    {
        m.SetAnnotation("disallowApplyingDefaultFormatting", "true");
    }
    else
    {
        m.RemoveAnnotation("disallowApplyingDefaultFormatting");
    }
}

foreach (var c in Selected.Columns)
{
    if (String.IsNullOrEmpty(c.GetAnnotation("disallowApplyingDefaultFormatting")))
    {
        c.SetAnnotation("disallowApplyingDefaultFormatting", "true");
    }
    else
    {
        c.RemoveAnnotation("disallowApplyingDefaultFormatting");
    }
}

ScriptHelper.Info("Script finished.");
