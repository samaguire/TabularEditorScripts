#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

// https://docs.tabulareditor.com/Useful-script-snippets.html#create-measures-from-columns

foreach(var c in Selected.Columns)
{
    var newMeasure = c.Table.AddMeasure(
        "Distinct Count Of " + c.Name,
        "DISTINCTCOUNT(" + c.DaxObjectFullName + ")",
        c.DisplayFolder + "\\Distinct Count Of Measures"
        );
    //c.IsHidden = true;
}

Info("Script finished.");
