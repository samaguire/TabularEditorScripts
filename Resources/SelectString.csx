#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using System.Windows.Forms;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***


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
