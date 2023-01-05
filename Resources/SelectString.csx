<<<<<<< HEAD
﻿#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Windows.Forms\v4.0_4.0.0.0__b77a5c561934e089\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***

=======
﻿#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.12\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
>>>>>>> main
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
using System.Windows.Forms;

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

Func<IList<string>, string, string> SelectString = (IList<string> listText, string titleText) =>
{

    var listboxText = new ListBox()
    {
        Dock = DockStyle.Fill
    };

    var panelButtons = new Panel()
    {
        Height = 22,
        Dock = DockStyle.Bottom
    };
    
    var buttonOK = new Button()
    {
        Text = "OK",
        DialogResult = DialogResult.OK,
        Left = 120
    };

    var buttonCancel = new Button()
    {
        Text = "Cancel",
        DialogResult = DialogResult.Cancel,
        Left = 204
    };

    var formInputBox = new Form()
    {
        Text = titleText,
        Padding = new System.Windows.Forms.Padding(8),
        FormBorderStyle = FormBorderStyle.FixedDialog,
        MinimizeBox = false,
        MaximizeBox = false,
        StartPosition = FormStartPosition.CenterScreen,
        AcceptButton = buttonOK,
        CancelButton = buttonCancel
    };

    listboxText.Items.AddRange(listText.ToArray());
    listboxText.SelectedItem = listText[0];
    formInputBox.Controls.AddRange(new Control[] { listboxText, panelButtons });
    panelButtons.Controls.AddRange(new Control[] { buttonOK, buttonCancel });

    return formInputBox.ShowDialog() == DialogResult.OK ? listboxText.SelectedItem.ToString() : null;

};

var stringSelected = SelectString(
    new string[] { "123", "abc", "456", "def", "789", "ghi" },
    "Select list item:"
    );

stringSelected.Output();