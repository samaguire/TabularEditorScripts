#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

string defaultQuery =
    "TOPN( 500, VALUES( <column> ) )\r\n" +
    "ORDER BY <column>";

foreach (var c in Selected.Columns)
{
    string daxQuery = defaultQuery.Replace("<column>", c.DaxObjectFullName);
    EvaluateDax(daxQuery).Output();
}
