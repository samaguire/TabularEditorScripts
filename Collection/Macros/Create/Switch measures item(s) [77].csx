#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.11\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/

var cg = null as CalculationGroupTable;

if (!ScriptHost.Model.CalculationGroups.Where(x => x.Name == "Switch Measures").Any())
{

    cg = ScriptHost.Model.AddCalculationGroup("Switch Measures");
    cg.CalculationGroupPrecedence = 99;
    cg.Columns["Name"].Name = "Switch Measures";

    if (!ScriptHost.Model.AllMeasures.Where(x => x.Name == "Switch Measure").Any())
    {
        cg.AddMeasure("Switch Measure", "ERROR(\"Please use with the 'Switch Measures' calculation group.\")");
    }

}
else
{
    cg = ScriptHost.Model.Tables["Switch Measures"] as CalculationGroupTable;
}

foreach (var m in ScriptHost.Selected.Measures)
{
    if (!cg.CalculationItems.Where(x => x.Name == m.Name).Any()) { cg.AddCalculationItem(m.Name, m.DaxObjectName); }
}

ScriptHost.Info("Script finished.");