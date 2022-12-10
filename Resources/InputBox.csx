#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.11\ref\net6.0\System.Windows.Forms.dll"
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

var stringDefault = "Time Intelligence";

stringDefault = InputBox(
    "Provide the common name for time/period intelligence tables. This is used to determine the calculation group's suffix, e.g. '(ISO)' in 'Time Intelligence (ISO)'.",
    "Default Time Intelligence Name",
    stringDefault
    );

stringDefault.Output();
