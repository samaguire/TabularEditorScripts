#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// https://docs.tabulareditor.com/Useful-script-snippets.html#create-measures-from-columns

foreach(var c in Selected.Columns)
{
    var newMeasure = c.Table.AddMeasure(
        "Avg Of " + c.Name,
        "AVERAGE( " + c.DaxObjectFullName + " )",
        c.DisplayFolder + "\\Avg Of Measures"
        );
    c.IsHidden = true;
}

ScriptHelper.Info("Script finished.");
