#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.7\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/

using System.Windows.Forms;

// Variables

var promptForVariables = false;                         // Set to true to prompt the user for the defaultTimeIntelligenceName & defaultCurrentPeriodItemName
var defaultTimeIntelligenceName = "Time Intelligence";  // Used to determine the calculation group's suffix
var defaultCurrentPeriodItemName = "CUR";               // Calculation item to excluded from measure creation
var defaultMeasureExpression =                          // The template DAX query
    "\r\n" +
    "CALCULATE(\r\n" +
    "    <measure>,\r\n" +
    "    <column> = \"<item>\"\r\n" +
    ")";

// Custom InputBox (instead of VB InputBox as this returns null instead of "" on cancel)

Func<string, string, string, string> InputBox = (string promptText, string titleText, string defaultText) =>
{

    var labelText = new Label()
    {
        Text = promptText,
        Dock = DockStyle.Fill,
    };

    var textboxText = new TextBox()
    {
        Text = defaultText,
        Dock = DockStyle.Bottom
    };

    var panelButtons = new Panel()
    {
        Height = 30,
        Dock = DockStyle.Bottom
    };
    
    var buttonOK = new Button()
    {
        Text = "OK",
        DialogResult = DialogResult.OK,
        Top = 8,
        Left = 120
    };

    var buttonCancel = new Button()
    {
        Text = "Cancel",
        DialogResult = DialogResult.Cancel,
        Top = 8,
        Left = 204
    };

    var formInputBox = new Form()
    {
        Text = titleText,
        Height = 143,
        Padding = new System.Windows.Forms.Padding(8),
        FormBorderStyle = FormBorderStyle.FixedDialog,
        MinimizeBox = false,
        MaximizeBox = false,
        StartPosition = FormStartPosition.CenterScreen,
        AcceptButton = buttonOK,
        CancelButton = buttonCancel
    };

    formInputBox.Controls.AddRange(new Control[] { labelText, textboxText, panelButtons });
    panelButtons.Controls.AddRange(new Control[] { buttonOK, buttonCancel });

    return formInputBox.ShowDialog() == DialogResult.OK ? textboxText.Text : null;

};

// Check measure(s) are selected

if (!ScriptHost.Selected.Measures.Any())
{
    ScriptHost.Error("No measure(s) ScriptHost.Selected.");
    return;
}

// Get variables input

if (promptForVariables)
{

    defaultTimeIntelligenceName = InputBox(
        "Provide the common name for time/period intelligence tables. This is used to determine the calculation group's suffix, e.g. '(ISO)' in 'Time Intelligence (ISO)'.",
        "Default Time Intelligence Name",
        defaultTimeIntelligenceName
        );
    if (defaultTimeIntelligenceName == null) { return; }

    defaultCurrentPeriodItemName = InputBox(
        "Provide the name of the 'current' time/period intelligence calculation item. Any calculation item with this name will be excluded from measure creation.",
        "Default 'Current' Name",
        defaultCurrentPeriodItemName
        );
    if (defaultCurrentPeriodItemName == null) { return; }

}

// Get calculation group table

var ts = ScriptHost.Model.Tables.Where(x => x.ObjectType == (ObjectType.CalculationGroupTable));
var t = null as CalculationGroupTable;

if (ts.Any())
{
    t = ScriptHost.SelectTable(ts, label:"Select calculation group table:") as CalculationGroupTable;
    if (t == null) { return; }
}
else
{
    ScriptHost.Error("No calculation group tables in the ScriptHost.Model.");
}

// Get calculation group's calculation items data column

var cs = t.DataColumns.Where(x => x.SourceColumn == "Name");
var c = null as DataColumn;

if (cs.Count() != 1)
{
    ScriptHost.Warning("Cannot identify calculation items column.");
    c = ScriptHost.SelectColumn(t, label:"Select calculation items column:") as DataColumn;
    if (c == null) { return; }
}
else
{
    c = cs.First();
}

// If calculation group is a time intelligence calculation group get the suffix (if any)

var tableSuffix = "" as string;
if (defaultTimeIntelligenceName.Length < t.Name.Length &&
    t.Name.ToUpper().Substring(0, defaultTimeIntelligenceName.Length) == defaultTimeIntelligenceName.ToUpper())
{
    tableSuffix = " " + t.Name.Substring(defaultTimeIntelligenceName.Length).Trim();
}

// Create measures

foreach (var m in ScriptHost.Selected.Measures)
{

    bool isCalculationGroupMeasure = Convert.ToBoolean(m.GetAnnotation("isCalculationGroupMeasure"));
    if (isCalculationGroupMeasure) { continue; }

    foreach (var i in t.CalculationItems.Where(i => !i.Name.ToUpper().Contains(defaultCurrentPeriodItemName.ToUpper())))
    {

        var measureName = m.Name + " " + i.Name + tableSuffix;
        var measureExpression = defaultMeasureExpression
            .Replace("<measure>", m.DaxObjectName)
            .Replace("<column>", c.DaxObjectFullName)
            .Replace("<item>", i.Name);
        var measureDisplayFolder = m.DisplayFolder + "\\🢒" + t.Name + "\\" + m.Name;

        var mms = ScriptHost.Model.AllMeasures.Where(x => x.Name == measureName);
        if (mms.Any()) { foreach (var mm in mms.ToList()) { mm.Delete(); } }

        var nm = m.Table.AddMeasure(measureName, measureExpression, measureDisplayFolder);

        var e = i.FormatStringExpression;
        if (String.IsNullOrEmpty(e))
        {
            // Format string expression is blank
            nm.FormatString = m.FormatString;
        }
        else if (e.Trim('\r', '\n', '"').Length < e.Length)
        {
            // Format string expression is a string
            nm.FormatString = e.Trim('\r', '\n', '"');
        }
        // todo #2 Add in process to handle DAX expressions e.g. https://www.esbrina-ba.com/showing-measure-descriptions-in-tooltips/

        nm.SetAnnotation("isCalculationGroupMeasure", "true");

        // todo #3 Add in logic to create descriptions

    }
}

// End

ScriptHost.Info("Script finished.");
