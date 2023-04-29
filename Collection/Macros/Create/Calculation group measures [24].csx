#load "..\..\..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.VisualBasic"

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
using Microsoft.VisualBasic;

// Check measure(s) are selected
if (!Selected.Measures.Any())
{
    ScriptHelper.Error("No measure(s) Selected.");
    return;
}

// Get calculation group table
var ts = Model.Tables.Where(x => x.ObjectType == (ObjectType.CalculationGroupTable)).Where(x => x.Name != "getSelectedMeasureFormatString");
var t = null as CalculationGroupTable;
if (ts.Any())
{
    t = ScriptHelper.SelectTable(ts, label: "Select calculation group table:") as CalculationGroupTable;
    if (t == null) { return; }
}
else
{
    ScriptHelper.Error("No calculation group tables in the Model.");
}

// Get calculation group's calculation items data column
var cs = t.DataColumns.Where(x => x.SourceColumn == "Name");
var c = null as DataColumn;
if (cs.Count() != 1)
{
    ScriptHelper.Warning("Cannot identify calculation items column.");
    c = ScriptHelper.SelectColumn(t, label: "Select calculation items column:") as DataColumn;
    if (c == null) { return; }
}
else
{
    c = cs.First();
}

// Update model's compatibility level if required
if (Model.Database.CompatibilityLevel < 1601)
{
    Model.Database.CompatibilityLevel = 1601;
}

// Create helper calculation group table, deleting existing table if it exists
if(Model.Tables.Where(x => x.Name == "getSelectedMeasureFormatString").Any()) { Model.Tables["getSelectedMeasureFormatString"].Delete(); }
var cg = Model.AddCalculationGroup(
    name: "getSelectedMeasureFormatString"
    );
cg.AddCalculationItem(
    name: "getSelectedMeasureFormatString",
    expression: "SELECTEDMEASUREFORMATSTRING()"
    );
cg.IsHidden = true;
cg.IsPrivate= true;

// Set template measure expression
var templateMeasureExpression = @"
CALCULATE(
    <m>,
    <c> = ""<i>""
)";

// Set template measure format string expression
var templateMeasureFormatStringExpression = @"
VAR CalculationItemFormat =
    CALCULATE(
        CALCULATE(
            <m>,
            <c> = ""<i>""
        ),
        'getSelectedMeasureFormatString'[Name] = ""getSelectedMeasureFormatString""
    )
VAR MeasureFormat =
    CALCULATE(
        <m>,
        'getSelectedMeasureFormatString'[Name] = ""getSelectedMeasureFormatString""
    )
RETURN
    CONVERT(
        COALESCE(CalculationItemFormat, MeasureFormat),
        STRING
    )";

// Cycle through selected measures
foreach (var m in Selected.Measures)
{

    // If current measure was derived from a calculation group then continue to the next measure
    bool isCalculationGroupMeasure = Convert.ToBoolean(m.GetAnnotation("isCalculationGroupMeasure"));
    if (isCalculationGroupMeasure) { continue; }

    // Cycle through calculation group items
    foreach (var i in t.CalculationItems)
    {

        // Define core measure properties
        var measureName = m.Name + " " + i.Name;
        var measureExpression = templateMeasureExpression
            .Replace("<m>", m.DaxObjectName)
            .Replace("<c>", c.DaxObjectFullName)
            .Replace("<i>", i.Name);
        var measureDisplayFolder = m.DisplayFolder + "\\› " + t.Name + "\\› " + m.Name;

        // Add measure to the model, deleting existing measure if it exists
        foreach (var mm in Model.AllMeasures.Where(x => x.Name == measureName).ToList()) { mm.Delete(); }
        var nm = m.Table.AddMeasure(
            name: measureName,
            expression: measureExpression,
            displayFolder: measureDisplayFolder
            );

        // Flag the new measure as derived from a calculation group
        nm.SetAnnotation("isCalculationGroupMeasure", "true");

        // Set the new measure's format string expression to derive from either the calculation group item or the source measure
        nm.FormatStringExpression = templateMeasureFormatStringExpression
            .Replace("<m>", m.DaxObjectName)
            .Replace("<c>", c.DaxObjectFullName)
            .Replace("<i>", i.Name);

        // Use the descriptions from both the source measure and calculation group item
        if (!string.IsNullOrEmpty(m.Description) && !string.IsNullOrEmpty(i.Description))
        {
            nm.Description = m.Description + "\n\r--\n\r" + i.Description;
        }
        else
        {
            nm.Description = m.Description + i.Description;
        }

    }
}

ScriptHelper.Info("Script finished.");