#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// https://github.com/bernatagulloesbrina/Quick-Actions

foreach (var c in Selected.Columns)
{
    if (c.DataType != DataType.Decimal && c.DataType != DataType.Double && c.DataType != DataType.Int64)
    {
        if (String.IsNullOrEmpty(c.GetExtendedProperty("ParameterMetadata")))
        {
            c.SetExtendedProperty("ParameterMetadata", "{\"version\":0}", ExtendedPropertyType.Json);
        }
        else
        {
            c.RemoveExtendedProperty("ParameterMetadata");
        }
    }
}

Info("Script finished.");
