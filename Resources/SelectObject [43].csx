#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

var selectedObject = ScriptHelper.SelectObject(
    new TabularNamedObject[]
    {
        Model.Tables["Account"],
        (Model.Tables["Dates"].Columns["Date"] as DataColumn),
        Model.Tables["Transactional"].Measures["Total Actual COGS"]
    }
    );

selectedObject.Output();
