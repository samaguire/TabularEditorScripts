#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

if (!Selected.Measures.Any()) { return; }

var cg = null as CalculationGroupTable;

if (!Model.CalculationGroups.Where(x => x.Name == "UX Switch").Any())
{
    cg = Model.AddCalculationGroup("UX Switch");
    cg.IsHidden = true;
    cg.CalculationGroupPrecedence = 99;
    cg.Columns["Name"].Name = "Measure";
}
else
{
    cg = Model.Tables["UX Switch"] as CalculationGroupTable;
}

foreach (var m in Selected.Measures)
{
    if (!cg.CalculationItems.Where(x => x.Name == m.Name).Any()) { cg.AddCalculationItem(m.Name, m.DaxObjectName); }
}

ScriptHelper.Info("Script finished.");
