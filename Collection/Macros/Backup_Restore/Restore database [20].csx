#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Windows.Forms\v4.0_4.0.0.0__b77a5c561934e089\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.AnalysisServices.Core.dll"

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
using Microsoft.AnalysisServices.Core;
using System.Windows.Forms;

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

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

var dbName = InputBox(
    "Provide the name of the restored database. (This will overwrite the database if it already exists.)",
    "Set Database Name",
    Model.Database.Name
    );

if (dbName == null) { return; }

Model.Database.TOMDatabase.Server.Restore(
    file: Model.Database.Name + ".abf",
    databaseName: dbName,
    allowOverwrite: true
);

ScriptHelper.Info("Script finished.");