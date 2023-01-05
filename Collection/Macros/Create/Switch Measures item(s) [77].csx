#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.12\ref\net6.0\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.UI;
using TabularEditor.Scripting;
// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

var cg = null as CalculationGroupTable;

if (!Model.CalculationGroups.Where(x => x.Name == "Switch Measures").Any())
{

    cg = Model.AddCalculationGroup("Switch Measures");
    cg.CalculationGroupPrecedence = 99;
    cg.Columns["Name"].Name = "Switch Measures";

    if (!Model.AllMeasures.Where(x => x.Name == "Switch Measure").Any())
    {
        cg.AddMeasure("Switch Measure", "ERROR(\"Please use with the 'Switch Measures' calculation group.\")");
    }

}
else
{
    cg = Model.Tables["Switch Measures"] as CalculationGroupTable;
}

foreach (var m in Selected.Measures)
{
    if (!cg.CalculationItems.Where(x => x.Name == m.Name).Any()) { cg.AddCalculationItem(m.Name, m.DaxObjectName); }
}

ScriptHelper.Info("Script finished.");