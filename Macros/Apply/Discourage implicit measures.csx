#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

foreach (var c in Model.AllColumns)
{
    if (c.SummarizeBy != AggregateFunction.None)
    {
        c.SummarizeBy = AggregateFunction.None;
        c.SetAnnotation("SummarizationSetBy", "User");
    }
}

Model.DiscourageImplicitMeasures = true;

ScriptHelper.Info("Script finished.");
