#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

foreach (var c in Model.AllColumns.Where(x => x.SummarizeBy != AggregateFunction.None))
{
    c.SummarizeBy = AggregateFunction.None;
    c.SetAnnotation("SummarizationSetBy", "User");
}

Model.DiscourageImplicitMeasures = true;

ScriptHelper.Info("Script finished.");
